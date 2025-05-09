// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using Spatial.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for blockades.
/// </summary>
public static class BlockadeListLoader
{
    /// <summary>
    /// Load a <see cref="Blockade"/> list.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <returns>A <see cref="Blockade"/> list.</returns>
    public static List<Blockade> Load(string root, string path)
    {
        var data = File.ReadAllBytes(path);
        var blockades = new List<Blockade>();

        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        var count = reader.ReadUInt32();
        var blockSize = 0;

        for (var i = 0; i < count; i++)
        {
            var name = reader.ReadString(32);

            var blockade = new Blockade
            {
                Path = path,
                Name = Path.Join(Path.GetRelativePath(root, path), name),
                Hash = data.ToMD5(),
                Id = name,
                Start = new Point2D(reader.ReadUInt32(), reader.ReadUInt32()),
                End = new Point2D(reader.ReadUInt32(), reader.ReadUInt32()),
                Size = reader.ReadUInt32(),
                Address = reader.ReadUInt32()
            };

            blockade.Data = new byte[blockade.Size * 2];
            blockades.Add(blockade);

            blockSize += blockade.Data.Length;   
        }

        var blocks = reader.ReadBytes(blockSize);

        for (var i = 0; i < blockades.Count; i++)
        {
            var blockade = blockades[i];

            Buffer.BlockCopy(blocks, (int) blockade.Address, blockade.Data, 0, blockade.Data.Length);
        
            blockade.Blocks = new Blockmap(
                width: blockade.Width / 8, 
                height: blockade.Height, 
                blocks: blockade.Data)
                {
                    Path = blockade.Path,
                    Name = blockade.Name,
                    Hash = blockade.Hash
                };
        }
        
        return blockades;
    }
}
