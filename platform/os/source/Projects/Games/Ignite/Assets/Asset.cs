// Copyright © Spatial. All rights reserved.

using Ignite.Assets.Loaders;
using Spatial.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ignite.Assets;

public class Asset
{
    private static readonly ConcurrentDictionary<string, Asset> _assets;
    private static readonly Dictionary<string, Type> _types;

    static Asset()
    {
        _assets = [];
        _types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(type => type.GetCustomAttribute<NameAttribute>() != null)
            .ToDictionary(type => type.GetCustomAttribute<NameAttribute>()!.Name);
    }
    
    /// <summary>
    /// The path to the <see cref="Asset"/>.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The name of the <see cref="Asset"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The hash of the <see cref="Asset"/>.
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// Get a required <see cref="Asset"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Asset"/> to get.</typeparam>
    /// <param name="name">The name of the <see cref="Asset"/>.</param>
    /// <returns>A <see cref="Asset"/> of type <typeparamref name="T"/>.</returns>
    public static T Require<T>(string name) where T : Asset
    {
        return (T) _assets.First(kvp => kvp.Key.ToLower().Matches(name.ToLower().ToNormalizedPath())).Value;
    }

    /// <summary>
    /// Get a <see cref="Asset"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Asset"/> to get.</typeparam>
    /// <param name="name">The name of the <see cref="Asset"/>.</param>
    /// <returns>A <see cref="Asset"/> of type <typeparamref name="T"/>, or null if it doesn't exist.</returns>
    public static T? Get<T>(string name) where T : Asset
    {
        if (_assets.FirstOrDefault(kvp => kvp.Key.ToLower().Matches(name.ToLower().ToNormalizedPath())).Value is not T asset)
        {
            return default;
        }

        return asset;
    }

    /// <summary>
    /// Get a list of assets.
    /// </summary>
    /// <typeparam name="T">The type of assets to list.</typeparam>
    /// <param name="name">The name of the asset to list.</param>
    /// <param name="filter">A filter for the search.</param>
    /// <returns>A list of assets.</returns>
    public static IEnumerable<T> List<T>(string name, Expression<Func<T, bool>>? filter = default) where T : Asset
    {
        return _assets
            .Where(kvp => 
                kvp.Key.ToLower().Matches(name.ToLower().ToNormalizedPath()) && 
                kvp.Value is T value && 
                (filter?.Compile().Invoke(value) ?? true))
            .Select(kvp => (T) kvp.Value);
    }

    /// <summary>
    /// Find the first record in a table.
    /// </summary>
    /// <typeparam name="T">The type of record to find.</typeparam>
    /// <param name="name">The name of the enclosing <see cref="Asset"/>.</param>
    /// <param name="filter">An optional search filter.</param>
    /// <returns>A record of type <typeparamref name="T"/>.</returns>
    public static T First<T>(string name, Expression<Func<T, bool>>? filter = default) => View(name, filter).First();

    /// <summary>
    /// Find the first record in a table.
    /// </summary>
    /// <typeparam name="T">The type of record to find.</typeparam>
    /// <param name="name">The name of the enclosing <see cref="Asset"/>.</param>
    /// <param name="filter">An optional search filter.</param>
    /// <returns>A record of type <typeparamref name="T"/>.</returns>
    public static T? FirstOrDefault<T>(string name, Expression<Func<T, bool>>? filter = default) => View(name, filter).FirstOrDefault();

    /// <summary>
    /// List table records.
    /// </summary>
    /// <typeparam name="T">The type of record to find.</typeparam>
    /// <param name="name">The name of the enclosing <see cref="Asset"/>.</param>
    /// <param name="filter">An optional search filter.</param>
    /// <returns>A list of records of type <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> View<T>(string name, Expression<Func<T, bool>>? filter = default)
    {
        if (_assets.FirstOrDefault(kvp => kvp.Key.ToLower().Matches(name.ToLower().ToNormalizedPath())).Value is not Table table)
        {
            return [];
        }

        var records = table.Records.Cast<T>();

        if (filter != null)
        {
            return records.Where(filter.Compile());
        }

        return records;
    }

    /// <summary>
    /// Load an <see cref="Asset"/>.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="path"></param>
    public static void Load(string root, string path)
    {
        switch (System.IO.Path.GetExtension(path))
        {
            case ".shn":
                Add(TableLoader.Load(root, path, _types));
                break;
            case ".txt":
                foreach (var table in TableListLoader.Load(root, path, _types))
                {
                    Add(table);
                }

                break;
            case ".bmp":
                Add(BitmapLoader.Load(root, path));
                break;
            case ".aid":
                foreach (var area in AreaListLoader.Load(root, path))
                {
                    Add(area);
                }

                break;
            case ".sbi":
                foreach (var blockade in BlockadeListLoader.Load(root, path))
                {
                    Add(blockade);
                }

                break;
            case ".shab":
            case ".shbd":
                Add(BlockmapLoader.Load(root, path));
                break;
            case ".cs":
            case ".ps":
            case ".lua":
                break;
        }
    }

    private static void Add(Asset asset)
    {
        _assets[asset.Name] = asset;
    }
}