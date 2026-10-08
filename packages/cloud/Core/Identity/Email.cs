// Copyright © Spatial Corporation. All rights reserved.

using System.Net.Mail;

namespace Spatial.Identity;

/// <summary>
/// A normalized email address.
/// </summary>
public readonly record struct Email
{
    /// <summary>
    /// The normalized address.
    /// </summary>
    /// <value>
    /// A trimmed, invariant-lowercased address. Guaranteed non-null and
    /// syntactically valid for every instance that exists.
    /// </value>
    public string Value { get; }

    /// <summary>
    /// Initialize a new <see cref="Email"/>.
    /// </summary>
    /// <param name="value">A normalized address.</param>
    private Email(string value) => Value = value;

    /// <summary>
    /// Normalize and validate an email address.
    /// </summary>
    public static Email Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        var normalized = value.Trim().ToLowerInvariant();

        try
        {
            _ = new MailAddress(normalized);
        }
        catch (FormatException)
        {
            throw new ArgumentException($"'{value}' is not a valid email address.", nameof(value));
        }

        return new Email(normalized);
    }

    /// <summary>
    /// Get the normalized address.
    /// </summary>
    /// <returns>The value of <see cref="Value"/>.</returns>
    public override string ToString() => Value;

    /// <summary>
    /// Convert an <see cref="Email"/> to its normalized string form.
    /// </summary>
    public static implicit operator string(Email email) => email.Value;
}