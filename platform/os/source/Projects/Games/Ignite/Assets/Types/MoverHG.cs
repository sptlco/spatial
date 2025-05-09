// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("MoverHG.shn")]
public class MoverHG
{
	public string MoverIDX { get; set; }

	public string FeedType { get; set; }

	public ushort RestoreAmount { get; set; }

	public ushort MaxHG { get; set; }

	public ushort CreateHG { get; set; }

	public ushort Tick { get; set; }

	public ushort ConsumeHG { get; set; }
}
