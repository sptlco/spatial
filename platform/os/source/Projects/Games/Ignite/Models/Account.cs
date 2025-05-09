// Copyright © Spatial. All rights reserved.

using MongoDB.Bson.Serialization.Attributes;
using Spatial.Extensions;
using Spatial.Persistence;
using Spatial.Structures;

namespace Ignite.Models;

/// <summary>
/// A unique user identity.
/// </summary>
[DocumentCollection(Collection.Accounts)]
public class Account : Document
{
    /// <summary>
    /// The account's username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The account's password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// The account's authorization level.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// A list of characters belonging to the <see cref="Account"/>.
    /// </summary>
    [BsonIgnore]
    public SparseArray<Character> Characters { get; set; }

    /// <summary>
    /// Create a new <see cref="Account"/>.
    /// </summary>
    public Account()
    {
        Characters = new SparseArray<Character>(6);
    }

    /// <summary>
    /// Create a new <see cref="Account"/>.
    /// </summary>
    /// <param name="username">The account's username.</param>
    /// <param name="password">The account's password.</param>
    /// <returns>An <see cref="Account"/>.</returns>
    public static Account Create(string username, string password)
    {
        var account = new Account {
            Username = username,
            Password = password
        };

        account.Store();

        return account;
    }

    /// <summary>
    /// Load an <see cref="Account"/>.
    /// </summary>
    /// <returns>An <see cref="Account"/>.</returns>
    public Account Load()
    {
        var characters = Document<Character>.List(c => c.Owner == Id);

        foreach (var character in characters)
        {
            Characters[character.Slot] = character.Load();
        }

        return this;
    }
}