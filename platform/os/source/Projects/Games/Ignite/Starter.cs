// Copyright © Spatial. All rights reserved.

using Ignite.Assets;
using Ignite.Assets.Types;
using Ignite.Models;
using Spatial.Compute.Jobs;
using Spatial.Extensions;
using Spatial.Persistence;
using Spatial.Structures;
using System;
using System.IO;
using System.Reflection.Metadata;

namespace Ignite;

/// <summary>
/// A global means to system initialization.
/// </summary>
public static class Starter
{
    /// <summary>
    /// Invoke the <see cref="Starter"/>.
    /// </summary>
    public static void Invoke(string[] args)
    {
        OverrideArguments(args);

        var progress = new Spinner();
        var path = Constants.Root;

        if (Directory.Exists(path))
        {
            Job.ParallelFor(Directory.GetFiles(path, "*", SearchOption.AllDirectories), file =>
            {
                progress.Spin($"Loading {Path.GetRelativePath(path, file)}");
                Asset.Load(path, file);
            });

            Job.ParallelFor(Field.List(), field =>
            {
                progress.Spin($"Creating {field.MapIDClient}");

                var maps = World.Maps[field.Serial] = new SparseArray<Map>(field.To - field.From + 1);

                if (field.Type == FIELD_MAP_TYPE.FMT_NORMAL)
                {
                    for (var id = 0; id < maps.Capacity; id++)
                    {
                        Map.Load(field, id);
                    }
                }
            });
        }

        progress.Dispose();
    }

    private static void OverrideArguments(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i].ToLower())
            {
                case "-d":
                case "--dir":
                    Constants.Root = args[++i].Replace("\"", "");
                    continue;
            }
        }
    }
}