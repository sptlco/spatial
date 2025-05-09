// Copyright © Spatial. All rights reserved.

namespace Ignite.Assets.Types;

[Name("Riding.shn")]
public class Riding
{
	public ushort Handle { get; set; }

	public string ItemID { get; set; }

	public string Name { get; set; }

	public string BodyType { get; set; }

	public string Shape { get; set; }

	public ushort UseTime { get; set; }

	public string FeedType { get; set; }

	public string Texture { get; set; }

	public ushort FeedGauge { get; set; }

	public ushort HGauge { get; set; }

	public ushort InitHgauge { get; set; }

	public ushort Tick { get; set; }

	public ushort UGauge { get; set; }

	public ushort RunSpeed { get; set; }

	public ushort FootSpeed { get; set; }

	public ushort CastingTime { get; set; }

	public uint CoolTime { get; set; }

	public string IconFileN { get; set; }

	public ushort IconIndex { get; set; }

	public string ImageN { get; set; }

	public string ImageH { get; set; }

	public string ImageE { get; set; }

	public string DummyA { get; set; }

	public string DummyB { get; set; }
}
