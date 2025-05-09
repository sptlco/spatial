// Copyright © Spatial. All rights reserved.

namespace Ignite.Contracts;

/// <summary>
/// Indicates the current status of a tutorial.
/// </summary>
public enum TUTORIAL_STATE
{
    /// <summary>
    /// The tutorial is in progress.
    /// </summary>
    TS_PROGRESS = 0x0,

    /// <summary>
    /// The tutorial has been completed.
    /// </summary>
    TS_DONE = 0x1,

    /// <summary>
    /// The tutorial was skipped.
    /// </summary>
    TS_SKIP = 0x2,

    /// <summary>
    /// An undocumented tutorial state.
    /// </summary>
    TS_EXCEPTION = 0x3,

    /// <summary>
    /// The maximum state of a tutorial.
    /// </summary>
    TS_MAX = 0x4
}