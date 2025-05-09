// Copyright © Spatial. All rights reserved.

using Spatial.Extensions;
using Spatial.Mathematics;
using System.Collections.Generic;
using System.IO;

namespace Ignite.Assets.Loaders;

/// <summary>
/// A loader for area lists.
/// </summary>
public static class AreaListLoader
{
    /// <summary>
    /// Load an <see cref="Area"/> list.
    /// </summary>
    /// <param name="root">The file's root directory.</param>
    /// <param name="path">The file's path, relative to <paramref name="root"/>.</param>
    /// <returns>A list of areas.</returns>
    public static List<Area> Load(string root, string path)
    {
        var data = File.ReadAllBytes(path);
        var list = new List<Area>();

        using var stream = new MemoryStream(data);
        using var reader = new BinaryReader(stream);

        var count = reader.ReadUInt32();

        for (var i = 0; i < count; i++)
        {
            var name = reader.ReadString(32);
            var type = (AreaType) reader.ReadInt32();

            switch (type)
            {
                case AreaType.Circle:
                    var circle = new Circle
                    {
                        Path = path,
                        Name = Path.Join(Path.GetRelativePath(root, path), name),
                        Hash = data.ToMD5(),
                        Id = name,
                        Position = new Point2D(reader.ReadSingle(), reader.ReadSingle()),
                        Radius = reader.ReadSingle()
                    };

                    list.Add(circle);
                    break;
                case AreaType.Rectangle:
                    var rectangle = new Rectangle
                    {
                        Path = path,
                        Name = Path.Join(Path.GetRelativePath(root, path), name),
                        Hash = data.ToMD5(),
                        Id = name,
                        Position = new Point2D(reader.ReadSingle(), reader.ReadSingle()),
                        Size = new Point2D(reader.ReadSingle(), reader.ReadSingle()),
                        Rotation = reader.ReadSingle()
                    };

                    list.Add(rectangle);
                    break;
            }
        }

        return list;
    }
}