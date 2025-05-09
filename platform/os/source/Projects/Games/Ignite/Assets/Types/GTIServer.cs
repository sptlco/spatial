// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("GTIServer.shn")]
public class GTIServer
{
	public byte ID { get; set; }

	public string InxName { get; set; }

	public uint SubjectTarget { get; set; }

	public byte EnemyNumber { get; set; }

	public uint GTIActionType { get; set; }

	public string Index { get; set; }

	public byte Value { get; set; }
}
