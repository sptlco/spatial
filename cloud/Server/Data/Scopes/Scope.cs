// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Scopes;

/// <summary>
/// ...
/// </summary>
public partial class Scope
{
    /// <summary>
    /// An identifier for the <see cref="Scope"/>.
    /// </summary>
    public string Tag { get; set; }
    
    /// <summary>
    /// The name of the <see cref="Scope"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The scope's icon.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// A description of the <see cref="Scope"/>.
    /// </summary>
    public string? Description { get; set; }
}