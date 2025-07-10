// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Compute;
using System.Runtime.CompilerServices;

namespace Spatial;

/// <summary>
/// The sole purpose of this class is to initialize Spatial, providing 
/// consumers of this library with a fully configured platform out of the box.
/// </summary>
internal class Module
{
    /// <summary>
    /// Initialize the <see cref="Module"/>.
    /// </summary>
    [ModuleInitializer]
    public static void Initialize()
    {
        Processor.Run();
    }
}
