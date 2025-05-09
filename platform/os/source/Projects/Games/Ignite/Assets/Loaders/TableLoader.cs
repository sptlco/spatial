// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for tables.
/// </summary>
public static class TableLoader
{
    /// <summary>
    /// Load a <see cref="Table"/>.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <param name="types">The global type map.</param>
    /// <returns>A <see cref="Table"/>.</returns>
    public static Table Load(string root, string path, IDictionary<string, Type>? types = default)
    {
        types ??= new Dictionary<string, Type>();

        var data = File.ReadAllBytes(path);

        if (path.Matches("*QuestData.shn"))
        {
            return LoadQuests(root, path, types);
        }
        
        Decrypt(new ArraySegment<byte>(
            array: data, 
            offset: 36, 
            count: data.Length - 36));

        var table = new Table
        {
            Path = path,
            Name = Path.GetRelativePath(root, path),
            Hash = data.ToMD5()
        };
        
        using var stream = new MemoryStream(data, 36, data.Length - 36);
        using var reader = new BinaryReader(stream);

        reader.ReadBytes(4);

        var height = reader.ReadUInt32();
        _ = reader.ReadUInt32();
        var width = reader.ReadUInt32();

        var columns = new Dictionary<string, Column>();
        var currentColumnName = string.Empty;

        for (var i = 0; i < width; i++)
        {
            var columnName = reader.ReadString(48);
            var columnDataType = (DataType) reader.ReadUInt32();
            var columnDataSize = reader.ReadUInt32();

            if (!string.IsNullOrWhiteSpace(columnName))
            {
                currentColumnName = columnName;
                columns[currentColumnName] = new Column(columnName, columnDataType, columnDataSize);
            }
            else
            {
                columns[currentColumnName].Count++;
            }
        }

        for (var i = 0; i < height; i++)
        {
            var record = new Dictionary<string, object>();

            _ = reader.ReadUInt16();

            foreach (var column in columns.Values)
            {
                switch (column.Type)
                {
                    case DataType.BYTE:
                    case DataType.INXBYTE:
                        record[column.Name] = reader.ReadByte();
                        break;
                    case DataType.BYTE_BIT:
                        record[column.Name] = reader.ReadByte() != 0;
                        break;
                    case DataType.BYTE_ARRAY:
                    {
                        var array = new byte[column.Count];

                        for (var j = 0; j < column.Count; j++)
                        {
                            array[j] = reader.ReadByte();
                        }

                        record[column.Name] = array;
                        break;
                    }
                    case DataType.WORD:
                    case DataType.INXWORD:
                        record[column.Name] = reader.ReadUInt16();
                        break;
                    case DataType.WORD_BIT:
                        record[column.Name] = reader.ReadUInt16() != 0;
                        break;
                    case DataType.WORD_ARRAY:
                    {
                        var array = new ushort[column.Count];

                        for (var j = 0; j < column.Count; j++)
                        {
                            array[j] = reader.ReadUInt16();
                        }

                        record[column.Name] = array;
                        break;
                    }
                    case DataType.INX:
                    case DataType.DWORD:
                    case DataType.INXDWORD:
                        record[column.Name] = reader.ReadUInt32();
                        break;
                    case DataType.DWORD_BIT:
                        record[column.Name] = reader.ReadUInt32() != 0;
                        break;
                    case DataType.DWORD_ARRAY:
                    {
                        var array = new uint[column.Count];

                        for (var j = 0; j < column.Count; j++)
                        {
                            array[j] = reader.ReadUInt32();
                        }

                        record[column.Name] = array;
                        break;
                    }
                    case DataType.QWORD:
                    case DataType.INXQWORD:
                        record[column.Name] = reader.ReadUInt64();
                        break;
                    case DataType.QWORD_BIT:
                        record[column.Name] = reader.ReadUInt64() != 0;
                        break;
                    case DataType.QWORD_ARRAY:
                    {
                        var array = new ulong[column.Count];

                        for (var j = 0; j < column.Count; j++)
                        {
                            array[j] = reader.ReadUInt64();
                        }

                        record[column.Name] = array;
                        break;
                    }
                    case DataType.FLOAT:
                        record[column.Name] = reader.ReadSingle();
                        break;
                    case DataType.STR:
                    case DataType.INXSTR:
                    case DataType.STRAUTO:
                    case DataType.VARSTR:
                        record[column.Name] = reader.ReadString((int) column.Size);
                        break;
                    case DataType.STR_ARRAY:
                    case DataType.STRAUTO_ARRAY:
                    {
                        var array = new string[column.Count];

                        for (var j = 0; j < column.Count; j++)
                        {
                            array[j] = reader.ReadString((int) column.Size);
                        }

                        record[column.Name] = array;
                        break;
                    }
                    case DataType.KVP:
                        record[column.Name] = new KeyValuePair<uint, uint>(reader.ReadUInt32(), reader.ReadUInt32());
                        break;
                    default:
                        continue;
                }
            }
            
            if (types.TryGetValue(table.Name, out var type))
            {
                var recordJson = JsonSerializer.Serialize(record);
                var recordObj = JsonSerializer.Deserialize(recordJson, type)!;

                table.Records.Add(recordObj);
                continue;
            }

            table.Records.Add(record);
        }

        return table;
    }
    
    private static Table LoadQuests(string root, string path, IDictionary<string, Type> types)
    {
        // ...
        
        return new Table() { Name = "QuestData.shn" };
    }
    
    private static void Decrypt(ArraySegment<byte> data)
    {
        var xor = (byte) data.Count;

        for (var i = data.Count - 1; i >= 0; i--)
        {
            data[i] ^= xor;

            var next = (byte) i;
            next &= 0xF;
            next += 0x55;
            next ^= (byte) (i * 0xB);
            next ^= xor;
            next ^= 0xAA;

            xor = next;
        }
    }
}