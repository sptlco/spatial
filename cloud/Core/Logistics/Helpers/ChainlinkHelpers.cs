
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;

namespace Spatial.Logistics.Helpers;

/// <summary>
/// Fetches the latest round data.
/// </summary>
[Function("latestRoundData", typeof(LatestRoundDataOutputDTO))]
public class LatestRoundDataFunction : FunctionMessage
{
}

/// <summary>
/// Output from the <see cref="LatestRoundDataFunction"/>.
/// </summary>
public class LatestRoundDataOutputDTO : IFunctionOutputDTO
{
    /// <summary>
    /// The round's ID.
    /// </summary>
    [Parameter("uint80", "roundId", 1)]
    public ulong RoundId { get; set; }

    /// <summary>
    /// The price of the token.
    /// </summary>
    [Parameter("int256", "answer", 2)]
    public BigInteger Answer { get; set; }

    /// <summary>
    /// The time the round started.
    /// </summary>
    [Parameter("uint256", "startedAt", 3)]
    public BigInteger StartedAt { get; set; }

    /// <summary>
    /// The time the round was updated.
    /// </summary>
    [Parameter("uint256", "updatedAt", 4)]
    public BigInteger UpdatedAt { get; set; }

    /// <summary>
    /// ...
    /// </summary>
    [Parameter("uint80", "answeredInRound", 5)]
    public ulong AnsweredInRound { get; set; }
}