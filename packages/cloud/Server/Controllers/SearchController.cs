// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Cloud.Contracts.Search;

namespace Spatial.Cloud.Controllers;

/// <summary>
/// A <see cref="Controller"/> for search.
/// </summary>
[Path("search")]
public class SearchController : Controller
{
    /// <summary>
    /// Search the domain for structured data.
    /// </summary>
    /// <param name="options">Configurable options for the search.</param>
    /// <returns>The results of the search.</returns>
    [POST]
    [Path("/")]
    public async Task<SearchResults> SearchAsync([Body] SearchOptions options)
    {
        // ...

        return new SearchResults {
            // ...
        };
    }
}