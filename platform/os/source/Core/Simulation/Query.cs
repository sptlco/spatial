// Copyright © Spatial. All rights reserved.

namespace Spatial.Simulation;

/// <summary>
/// A search for entities based on component requirements.
/// </summary>
public partial class Query
{
    private bool _parallel;

    private Signature _all;
    private Signature _any;
    private Signature _none;

    /// <summary>
    /// Create a new <see cref="Query"/>.
    /// </summary>
    public Query()
    {
        _parallel = true;

        _all = new Signature();
        _any = new Signature();
        _none = new Signature();
    }

    /// <summary>
    /// Whether or not to process matching entities in parallel.
    /// </summary>
    public bool Accelerated => _parallel;

    /// <summary>
    /// Process matching entities in parallel.
    /// </summary>
    /// <param name="parallel">Whether or not to run the query in parallel.</param>
    /// <returns>The <see cref="Query"/> for chained method calls.</returns>
    public Query Parallel(bool parallel = true)
    {
        _parallel = parallel;
        return this;
    }

    /// <summary>
    /// Specifies that entities must have all of the specified component types.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/> requirement.</param>
    /// <returns>The <see cref="Query"/> for chained method calls.</returns>
    public Query WithAll(in Signature signature)
    {
        _all |= signature;
        return this;
    }

    /// <summary>
    /// Specifies that entities must have at least one of the specified component types.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/> requirement.</param>
    /// <returns>The <see cref="Query"/> for chained method calls.</returns>
    public Query WithAny(in Signature signature)
    {
        _any |= signature;
        return this;
    }

    /// <summary>
    /// Specifies that entities must not have any of the specified component types.
    /// </summary>
    /// <param name="signature">A <see cref="Signature"/> requirement.</param>
    /// <returns>The <see cref="Query"/> for chained method calls.</returns>
    public Query WithNone(in Signature signature)
    {
        _none |= signature;
        return this;
    }

    /// <summary>
    /// Determines if the given <see cref="Signature"/> matches the <see cref="Query"/> requirements.
    /// </summary>
    /// <param name="signature">The <see cref="Signature"/> to check against the <see cref="Query"/>.</param>
    /// <returns>True if the <see cref="Signature"/> matches the <see cref="Query"/> requirements; otherwise, false.</returns>
    public bool Matches(in Signature signature)
    {
        return signature.All(_all) && (_any.Count <= 0 || signature.Any(_any)) && signature.None(_none);
    }
}