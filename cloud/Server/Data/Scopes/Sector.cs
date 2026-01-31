// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data.Scopes;

/// <summary>
/// A group of scopes.
/// </summary>
public class Sector
{
    /// <summary>
    /// The name of the <see cref="Sector"/>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A list of scopes in the <see cref="Sector"/>.
    /// </summary>
    public List<Scope> Scopes { get; set; } = [];
}