// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Contracts.Search;

/// <summary>
/// Configurable options for a search.
/// </summary>
public class SearchOptions
{
    /// <summary>
    /// Optional words or phrases to search for.
    /// </summary>
    public string Keywords { get; set; } = string.Empty;
}