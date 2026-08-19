// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Search;

/// <summary>
/// Structural results of a search.
/// </summary>
public class SearchResults
{
    /// <summary>
    /// A list of matching objects.
    /// </summary>
    public List<string> Objects { get; set; } = [];

    /// <summary>
    /// A list of suggested keywords.
    /// </summary>
    public List<string> Keywords { get; set; } = [];
}