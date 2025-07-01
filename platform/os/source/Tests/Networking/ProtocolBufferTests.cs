// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking.Contracts.Miscellaneous;

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="ProtocolBuffer"/>.
/// </summary>
public class ProtocolBufferTests
{
    /// <summary>
    /// Test <see cref="ProtocolBuffer.FromSpan"/>.
    /// </summary>
    [Fact]
    public void TestFromSpan()
    {
        var buffer = ProtocolBuffer.FromSpan<PROTO_NC_MISC_SEED_CMD>([4, 0]);

        Assert.Equal(4, buffer.Seed);
    }

    /// <summary>
    /// Test <see cref="ProtocolBuffer.Serialize"/>.
    /// </summary>
    [Fact]
    public void TestSerialize()
    {
        var buffer = new TestBuffer(5);

        buffer.Serialize();

        var array = buffer.ToArray();

        Assert.Equal(4, array.Length);
        Assert.Equal(5, BitConverter.ToInt32(array));
    }

    /// <summary>
    /// Test <see cref="ProtocolBuffer.Deserialize"/>.
    /// </summary>
    [Fact]
    public void TestDeserialize()
    {
        var data = new byte[] {5, 0, 0, 0};
        var buffer = new TestBuffer();

        buffer.Stream.Write(data.AsSpan());
        buffer.Stream.Position = 0;

        buffer.Deserialize();

        Assert.Equal(5, buffer.Number);
    }

    /// <summary>
    /// Test <see cref="ProtocolBuffer.ToArray"/>.
    /// </summary>
    [Fact]
    public void TestToArray()
    {
        var buffer = new TestBuffer(5);

        buffer.Serialize();

        var array = buffer.ToArray();

        Assert.Equal(4, array.Length);
        Assert.Equal(5, BitConverter.ToInt32(array));
    }

    /// <summary>
    /// Test <see cref="ProtocolBuffer.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        new TestBuffer(5).Dispose();
    }

    private class TestBuffer : ProtocolBuffer
    {
        public TestBuffer() {}

        public TestBuffer(int number)
        {
            Number = number;
        }

        public int Number { get; set; }

        public override void Deserialize()
        {
            Number = ReadInt32();
        }

        public override void Serialize()
        {
            Write(Number);
        }
    }
}
