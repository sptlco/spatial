// Copyright © Spatial. All rights reserved.

namespace Ignite.Models;

/// <summary>
/// A dialog prompting the user to perform an action.
/// </summary>
public class Menu
{
    /// <summary>
    /// An item selected by the player.
    /// </summary>
    /// <param name="Title">The item's title.</param>
    /// <param name="Function">The item's <see cref="MenuFunction"/>.</param>
    public record struct Item(string Title, MenuFunction Function);
}

/// <summary>
/// A function executed when a <see cref="Menu.Item"/> is selected.
/// </summary>
public delegate void MenuFunction();