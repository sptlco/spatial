// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Cloud.Data;

/// <summary>
/// A postal address.
/// </summary>
public class Address
{
    /// <summary>
    /// The recipient's name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The recipient's company.
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// The first line of the street address.
    /// </summary>
    public string Line1 { get; set; }

    /// <summary>
    /// The second line of the street address.
    /// </summary>
    public string? Line2 { get; set; }

    /// <summary>
    /// The city.
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// The state or province.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// The postal code.
    /// </summary>
    public string Zip { get; set; }

    /// <summary>
    /// The country code.
    /// </summary>
    public string Country { get; set; }
}