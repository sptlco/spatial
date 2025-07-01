// Copyright © Spatial Corporation. All rights reserved.

using Spatial.Networking.Contracts.Miscellaneous;
using System.Net.Sockets;

namespace Spatial.Networking;

/// <summary>
/// Unit tests for <see cref="Connection"/>.
/// </summary>
public class ConnectionTests
{
    /// <summary>
    /// Test <see cref="Connection.Allocate"/>.
    /// </summary>
    [Fact]
    public void TestAllocate()
    {
        Assert.NotNull(CreateConnection());
    }

    /// <summary>
    /// Test <see cref="Connection.Connect"/>.
    /// </summary>
    [Fact]
    public void TestConnect()
    {
        Server.Open();

        try
        {
            CreateConnection().Connect();
        }
        catch (SocketException)
        {
        }
    }

    /// <summary>
    /// Test <see cref="Connection.Disconnect"/>.
    /// </summary>
    [Fact]
    public void TestDisconnect()
    {
        Server.Open();
        
        var connection = CreateConnection();

        try
        {
            connection.Connect();
            connection.Disconnect();
        }
        catch (SocketException)
        {
        }
    }

    /// <summary>
    /// Test <see cref="Connection.Command"/>.
    /// </summary>
    [Fact]
    public void TestCommand()
    {
        CreateConnection().Command(
            command: NETCOMMAND.NC_MISC_SEED_CMD,
            data: new PROTO_NC_MISC_SEED_CMD(50));
    }

    /// <summary>
    /// Test <see cref="Connection.Dispose"/>.
    /// </summary>
    [Fact]
    public void TestDispose()
    {
        try
        {
            CreateConnection().Dispose();
        }
        catch
        {
        }
    }

    internal static Connection CreateConnection()
    {
        return Connection.Allocate(CreateSocket());
    }

    private static Socket CreateSocket()
    {
        return new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
}
