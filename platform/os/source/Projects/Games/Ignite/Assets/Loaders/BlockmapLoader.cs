// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using System.IO;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for blockmaps.
/// </summary>
public static class BlockmapLoader
{
    /// <summary>
    /// Load a <see cref="Blockmap"/>.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <returns>A <see cref="Blockmap"/>.</returns>
    public static Blockmap Load(string root, string path)
    {
        var data = File.ReadAllBytes(path);

        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        var width = reader.ReadInt32();
        var height = reader.ReadInt32();
        var blocks = reader.ReadBytes(width * height);

        var blockmap = new Blockmap(width, height, blocks)
        {
            Path = path,
            Name = Path.GetRelativePath(root, path),
            Hash = data.ToMD5()
        };

        return blockmap;
    }
}