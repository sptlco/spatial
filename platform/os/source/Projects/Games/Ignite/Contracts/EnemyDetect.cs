// Copyright © Spatial. All rights reserved.

namespace Ignite.Contracts;

/// <summary>
/// Indicates a mob's enemy detection mode.
/// </summary>
public enum EnemyDetect
{
    ED_BOUT = 0x0,
    ED_AGGRESSIVE = 0x1,
    ED_NOBRAIN = 0x2,
    ED_AGGRESSIVE2 = 0x3,
    ED_AGGREESIVEALL = 0x4,
    ED_ENEMYALLDETECT = 0x5,
    MAX_ENEMYDETECT = 0x6
}
