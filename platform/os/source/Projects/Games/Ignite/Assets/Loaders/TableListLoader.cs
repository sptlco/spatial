// Copyright © Spatial. All rights reserved.

using Microsoft.VisualBasic.FileIO;
using Spatial.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for tables.
/// </summary>
public static class TableListLoader
{
    /// <summary>
    /// Load a <see cref="Table"/> list.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <param name="types">The global type map.</param>
    /// <returns>A <see cref="Table"/> list.</returns>
    public static List<Table> Load(string root, string path, IDictionary<string, Type>? types = default)
    {
        types ??= new Dictionary<string, Type>();
        
        var tables = new List<Table>();
        
        var intermediates = new ConcurrentDictionary<string, Table>();
        var tableColumns = new Dictionary<string, Dictionary<string, Column>>();

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var data = new byte[stream.Length];
        stream.ReadExactly(data, 0, (int) stream.Length);
        stream.Position = 0;

        using var reader = new TextFieldParser(stream);
        
        reader.CommentTokens = [";"];
        reader.HasFieldsEnclosedInQuotes = true;
        reader.TrimWhiteSpace = true;

        reader.SetDelimiters(",", "\t");

        var currentTable = default(Table?);
        var currentColumnIndex = 0;

        while (!reader.EndOfData)
        {
            var fields = reader
                .ReadFields()?
                .Where(str => !string.IsNullOrEmpty(str))
                .ToArray() ?? [];

            var directive = fields[0];
            var lowercaseDirective = directive.ToLower();

            switch (lowercaseDirective)
            {
                case "#table":
                case "#define":
                    var tableName = fields[1];
                    var table = new Table
                    {
                        Path = path,
                        Name = tableName,
                        Hash = data.ToMD5(),
                        Parent = Path.GetRelativePath(root, path)
                    };

                    currentTable = table;
                    intermediates[table.Name] = table;

                    tableColumns[table.Name] = [];
                    currentColumnIndex = 0;
                    break;
                case "#columntype":
                    if (currentTable != default)
                    {
                        for (var i = 1; i < fields.Length; i++)
                        {
                            var columnName = (i - 1).ToString();

                            if (!tableColumns[currentTable.Name].TryGetValue(columnName, out var column))
                            {
                                tableColumns[currentTable.Name][columnName] = column = new Column(columnName);
                            }

                            column.Type = ConvertColumnTypeToDataType(fields[i]);
                            tableColumns[currentTable.Name][columnName] = column;
                        }
                    }

                    break;
                case "#columnname":
                    if (currentTable != default)
                    {
                        for (var i = 1; i < fields.Length; i++)
                        {
                            var columnName = (i - 1).ToString();

                            if (!tableColumns[currentTable.Name].TryGetValue(columnName, out var column))
                            {
                                tableColumns[currentTable.Name][columnName] = column = new Column(DataType.UNKNOWN);
                            }

                            column.Name = fields[i];
                            tableColumns[currentTable.Name][columnName] = column;
                        }
                    }

                    break;
                case "<boolean>":
                case "bool":
                case "<byte>":
                case "byte":
                case "<short>":
                case "word":
                case "<integer>":
                case "dwrd":
                case "dword":
                case "<long>":
                case "qwrd":
                case "qword":
                case "<string>":
                case "string":
                case "index":
                    if (currentTable != default)
                    {
                        tableColumns[currentTable.Name][currentColumnIndex.ToString()] = new Column(currentColumnIndex.ToString(), ConvertColumnTypeToDataType(directive));
                        currentColumnIndex++;
                    }

                    break;
                default:
                    intermediates.TryGetValue(directive, out var recordTable);

                    recordTable ??= currentTable != default && lowercaseDirective.Equals("#record") ? currentTable : default;
                    recordTable ??= lowercaseDirective.Equals("#recordin") && intermediates.TryGetValue(fields[1], out recordTable) ? recordTable : default;

                    if (recordTable == default)
                    {
                        break;
                    }

                    var startIndex = lowercaseDirective.Equals("#recordin") ? 2 : 1;
                    var record = new Dictionary<string, object?>();
                    var recordColumns = tableColumns[recordTable.Name];

                    foreach (var (columnIndex, column) in recordColumns)
                    {
                        var valueIndex = startIndex + int.Parse(columnIndex);
                        var valueString = fields.ElementAtOrDefault(valueIndex) ?? string.Empty;

                        if (string.IsNullOrEmpty(valueString))
                        {
                            record[columnIndex] = default;
                            continue;
                        }

                        object? value = default;

                        switch (column.Type)
                        {
                            case DataType.BYTE_BIT:
                                if (bool.TryParse(valueString, out var boolValue))
                                {
                                    value = boolValue;
                                }
                                else if (int.TryParse(valueString, out var intValue))
                                {
                                    value = intValue == 1;
                                }
                                else
                                {
                                    value = default(bool);
                                }

                                break;
                            case DataType.BYTE:
                                value = byte.Parse(valueString);
                                break;
                            case DataType.WORD:
                                value = short.Parse(valueString);
                                break;
                            case DataType.DWORD:
                                value = int.Parse(valueString);
                                break;
                            case DataType.QWORD:
                                value = long.Parse(valueString);
                                break;
                            case DataType.STR:
                                value = valueString;
                                break;
                        }

                        record[columnIndex] = value;
                    }

                    var friendlyName = Regex.Replace(recordTable.Name, @"[\d-]", string.Empty);
                    var subPath = Path.Join(Path.GetRelativePath(root, path), friendlyName);
                    var typeName = types.Keys.FirstOrDefault(subPath.Matches);

                    if (!string.IsNullOrEmpty(typeName) && types.TryGetValue(typeName, out var type))
                    {
                        var recordJson = JsonSerializer.Serialize(record);
                        var recordObj = JsonSerializer.Deserialize(recordJson, type)!;

                        recordTable.Records.Add(recordObj);
                        break;
                    }

                    recordTable.Records.Add(record);
                    break;
            }
        }

        foreach (var table in intermediates.Values)
        {
            var parentPath = Path.GetRelativePath(root, path);
            var childPath = Path.Join(Path.GetRelativePath(root, path), table.Name);

            if (!intermediates.TryGetValue(parentPath, out var parent))
            {
                parent = intermediates[parentPath] = new Table
                {
                    Path = path,
                    Name = parentPath
                };
            }

            table.Name = childPath;

            parent.Children.Add(table);
            tables.Add(table);
        }

        return tables;
    }
    
    private static DataType ConvertColumnTypeToDataType(string type)
    {
        if (type.StartsWith("string[", StringComparison.CurrentCultureIgnoreCase))
        {
            return DataType.STR;
        }

        switch (type.ToLower())
        {
            case "<boolean>":
            case "bool":
                return DataType.BYTE_BIT;
            case "<byte>":
            case "byte":
                return DataType.BYTE;
            case "<short>":
            case "word":
                return DataType.WORD;
            case "<integer>":
            case "dwrd":
            case "dword":
                return DataType.DWORD;
            case "<long>":
            case "qwrd":
            case "qword":
                return DataType.QWORD;
            case "<string>":
            case "string":
            case "index":
                return DataType.STR;
            default:
                return DataType.UNKNOWN;
        }
    }
}