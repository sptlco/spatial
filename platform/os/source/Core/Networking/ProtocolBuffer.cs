// Copyright © Spatial. All rights reserved.

using System.Text;

namespace Spatial.Networking;

/// <summary>
/// An object used to transmit data over the network.
/// </summary>
public abstract class ProtocolBuffer : IDisposable
{
    private MemoryStream _stream;
    private BinaryReader _reader;
    private BinaryWriter _writer;

    /// <summary>
    /// Create a new <see cref="ProtocolBuffer"/>.
    /// </summary>
    public ProtocolBuffer()
    {
        _stream = new();
        _reader = new(_stream);
        _writer = new(_stream);
    }

    /// <summary>
    /// The buffer's <see cref="MemoryStream"/>.
    /// </summary>
    internal MemoryStream Stream => _stream;

    /// <summary>
    /// The size of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public long Size => _stream.Length;

    /// <summary>
    /// Create a <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ProtocolBuffer"/> to create.</typeparam>
    /// <param name="data">The bytes to deserialize the <see cref="ProtocolBuffer"/> from.</param>
    /// <returns>A <see cref="ProtocolBuffer"/>.</returns>
    public static T FromSpan<T>(Span<byte> data) where T : ProtocolBuffer
    {
        return (T) FromSpan(typeof(T), data);
    }

    /// <summary>
    /// Create a <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <param name="type">The type of <see cref="ProtocolBuffer"/> to create.</param>
    /// <param name="data">The bytes to deserialize the <see cref="ProtocolBuffer"/> from.</param>
    /// <returns>A <see cref="ProtocolBuffer"/>.</returns>
    public static ProtocolBuffer FromSpan(Type type, Span<byte> data)
    {
        var buffer = (ProtocolBuffer) Activator.CreateInstance(type)!;

        buffer.Stream.Write(data);
        buffer.Stream.Seek(0, SeekOrigin.Begin);

        buffer.Deserialize();

        return buffer;
    }

    /// <summary>
    /// Serialize the protocol buffer.
    /// </summary>
    /// <returns>Binary data.</returns>
    public abstract void Serialize();

    /// <summary>
    /// Deserialize the protocol buffer.
    /// </summary>
    public abstract void Deserialize();

    /// <summary>
    /// Convert the protocol buffer to an array.
    /// </summary>
    /// <returns>An array of bytes.</returns>
    public byte[] ToArray()
    {
        return _stream.ToArray();
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public virtual void Dispose()
    {
        _stream.Dispose();
        _reader.Dispose();
        _writer.Dispose();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Reads a single value of the specified unmanaged type from the data source.
    /// </summary>
    /// <typeparam name="T">The unmanaged type to read (supported types: byte, short, int, long, float, double).</typeparam>
    /// <returns>The read value of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected T Read<T>(int? size = default)
    {
        switch (typeof(T))
        {
            case Type t when t.IsAssignableTo(typeof(ProtocolBuffer)):
                var buffer = (Activator.CreateInstance<T>() as ProtocolBuffer)!;

                var stream = buffer._stream;
                var reader = buffer._reader;

                buffer._stream = _stream;
                buffer._reader = _reader;

                buffer.Deserialize();

                buffer._stream = stream;
                buffer._reader = reader;

                return (T) (object) buffer;
            case Type t when t == typeof(byte):
                return (T) (object) ReadByte();
            case Type t when t == typeof(char):
                return (T) (object) ReadChar();
            case Type t when t == typeof(short):
                return (T) (object) ReadInt16();
            case Type t when t == typeof(ushort):
                return (T) (object) ReadUInt16();
            case Type t when t == typeof(int):
                return (T) (object) ReadInt32();
            case Type t when t == typeof(uint):
                return (T) (object) ReadUInt32();
            case Type t when t == typeof(long):
                return (T) (object) ReadInt64();
            case Type t when t == typeof(ulong):
                return (T) (object) ReadUInt64();
            case Type t when t == typeof(float):
                return (T) (object) ReadSingle();
            case Type t when t == typeof(double):
                return (T) (object) ReadDouble();
            case Type t when t == typeof(string):
                return (T) (object) ReadString(size ?? (int) (_stream.Length - _stream.Position));
            default:
                throw new NotSupportedException($"Reading type {typeof(T).Name} is not supported.");
        }
    }

    /// <summary>
    /// Reads an array of values of the specified unmanaged type from the data source.
    /// </summary>
    /// <typeparam name="T">The unmanaged type of array elements.</typeparam>
    /// <param name="count">The number of elements to read.</param>
    /// <param name="size">The size of an element.</param>
    /// <returns>An array of read values of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected T[] Read<T>(int count, int? size = default)
    {
        var array = new T[count];

        for (var i = 0; i < count; i++)
        {
            array[i] = Read<T>(size);
        }
        
        return array;
    }

    /// <summary>
    /// Read an enumerable of values.
    /// </summary>
    /// <typeparam name="T">The type of elements to read.</typeparam>
    /// <param name="count">The number of elements to read.</param>
    /// <param name="size">The size of an element.</param>
    /// <returns>An enumerable of read values of type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected IEnumerable<T> ReadEnumerable<T>(int count, int? size = default)
    {
        for (var i = 0; i < count; i++)
        {
            yield return Read<T>(size);
        }
    }

    /// <summary>
    /// Read bytes.
    /// </summary>
    /// <returns>An array of bytes.</returns>
    protected byte[] ReadBytes()
    {
        return _reader.ReadBytes((int)(_stream.Length - _stream.Position));
    }

    /// <summary>
    /// Read bytes.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>An array of bytes.</returns>
    protected byte[] ReadBytes(int count)
    {
        return _reader.ReadBytes(count);
    }

    /// <summary>
    /// Read a <see cref="byte"/>.
    /// </summary>
    /// <returns>A <see cref="byte"/>.</returns>
    protected byte ReadByte()
    {
        return _reader.ReadByte();
    }

    /// <summary>
    /// Read a <see cref="sbyte"/>.
    /// </summary>
    /// <returns>A <see cref="sbyte"/>.</returns>
    protected sbyte ReadSByte()
    {
        return _reader.ReadSByte();
    }

    /// <summary>
    /// Read a <see cref="char"/>.
    /// </summary>
    /// <returns>A <see cref="char"/>.</returns>
    protected char ReadChar()
    {
        return _reader.ReadChar();
    }

    /// <summary>
    /// Read a <see cref="bool"/>.
    /// </summary>
    /// <returns>A <see cref="bool"/>.</returns>
    protected bool ReadBoolean()
    {
        return _reader.ReadBoolean();
    }

    /// <summary>
    /// Read a <see cref="ushort"/>.
    /// </summary>
    /// <returns>A <see cref="ushort"/>.</returns>
    protected ushort ReadUInt16()
    {
        return _reader.ReadUInt16();
    }

    /// <summary>
    /// Read a <see cref="short"/>.
    /// </summary>
    /// <returns>A <see cref="short"/>.</returns>
    protected short ReadInt16()
    {
        return _reader.ReadInt16();
    }

    /// <summary>
    /// Read a <see cref="uint"/>.
    /// </summary>
    /// <returns>A <see cref="uint"/>.</returns>
    protected uint ReadUInt32()
    {
        return _reader.ReadUInt32();
    }

    /// <summary>
    /// Read an <see cref="int"/>.
    /// </summary>
    /// <returns>An <see cref="int"/>.</returns>
    protected int ReadInt32()
    {
        return _reader.ReadInt32();
    }

    /// <summary>
    /// Read a <see cref="float"/>.
    /// </summary>
    /// <returns>A <see cref="float"/>.</returns>
    protected float ReadSingle()
    {
        return _reader.ReadSingle();
    }

    /// <summary>
    /// Read a <see cref="ulong"/>.
    /// </summary>
    /// <returns>A <see cref="ulong"/>.</returns>
    protected ulong ReadUInt64()
    {
        return _reader.ReadUInt64();
    }

    /// <summary>
    /// Read a <see cref="long"/>.
    /// </summary>
    /// <returns>A <see cref="long"/>.</returns>
    protected long ReadInt64()
    {
        return _reader.ReadInt64();
    }

    /// <summary>
    /// Read a <see cref="decimal"/>.
    /// </summary>
    /// <returns>A <see cref="decimal"/>.</returns>
    protected decimal ReadDecimal()
    {
        return _reader.ReadDecimal();
    }

    /// <summary>
    /// Read a <see cref="double"/>.
    /// </summary>
    /// <returns>A <see cref="double"/>.</returns>
    protected double ReadDouble()
    {
        return _reader.ReadDouble();
    }

    /// <summary>
    /// Read a <see cref="string"/>.
    /// </summary>
    /// <param name="length">The length of the <see cref="string"/>.</param>
    /// <returns>A <see cref="string"/>.</returns>
    protected string ReadString(int length)
    {
        var buffer = _reader.ReadBytes(length);
        var count = 0;

        if (buffer[length - 1] != 0)
        {
            count = length;
        }
        else
        {
            while (buffer[count] != 0 && count < buffer.Length)
            {
                count++;
            }
        }

        return count > 0 ? Encoding.ASCII.GetString(buffer, 0, count) : string.Empty;
    }

    /// <summary>
    /// Fill the <see cref="ProtocolBuffer"/> with an arbitrary value.
    /// </summary>
    /// <param name="count">The number of bytes to fill.</param>
    /// <param name="value">The value to fill the bytes with.</param>
    protected void Fill(int count, byte value)
    {
        for (var i = 0; i < count; i++)
        {
            Write(value);
        }
    }

    /// <summary>
    /// Writes a single value of the specified unmanaged type to the data destination.
    /// </summary>
    /// <typeparam name="T">The unmanaged type to write.</typeparam>
    /// <param name="value">The value to write of type <typeparamref name="T"/>.</param>
    /// <param name="size">The size of the value to write.</param>
    /// <param name="prefix">Whether or not to prefix the value with its length.</param>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected void Write<T>(T value, int? size = default, bool? prefix = false)
    {
        if (value == null)
        {
            throw new NotSupportedException("Writing null is not supported.");
        }

        switch (typeof(T))
        {
            case Type t when t.IsAssignableTo(typeof(ProtocolBuffer)):
                var buffer = (ProtocolBuffer) (object) value;

                var stream = buffer._stream;
                var writer = buffer._writer;

                buffer._stream = _stream;
                buffer._writer = _writer;

                buffer.Serialize();

                buffer._stream = stream;
                buffer._writer = writer;

                break;
            case Type t when t == typeof(byte):
                Write((byte) (object) value);
                break;
            case Type t when t == typeof(char):
                Write((char) (object) value);
                break;
            case Type t when t == typeof(bool):
                Write((bool) (object) value);
                break;
            case Type t when t == typeof(short):
                Write((short) (object) value);
                break;
            case Type t when t == typeof(ushort):
                Write((ushort) (object) value);
                break;
            case Type t when t == typeof(int):
                Write((int) (object) value);
                break;
            case Type t when t == typeof(uint):
                Write((uint) (object) value);
                break;
            case Type t when t == typeof(long):
                Write((long) (object) value);
                break;
            case Type t when t == typeof(ulong):
                Write((ulong) (object) value);
                break;
            case Type t when t == typeof(float):
                Write((float) (object) value);
                break;
            case Type t when t == typeof(double):
                Write((double) (object) value);
                break;
            case Type t when t == typeof(string):
                var str = (string) (object) value;
                Write(str, size ?? str.Length, prefix);
                break;
            default:
                throw new NotSupportedException($"Writing type {typeof(T).Name} is not supported.");
        }
    }

    /// <summary>
    /// Write an array of values of the specified unmanaged type to the data destination.
    /// </summary>
    /// <typeparam name="T">The unmanaged type of array elements.</typeparam>
    /// <param name="array">The array of values to write.</param>
    /// <param name="size">The size of the value to write.</param>
    /// <param name="prefix">Whether or not to prefix the value with its length.</param>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected void Write<T>(T[] array, int? size = default, bool? prefix = false)
    {
        foreach (var item in array)
        {
            Write(item, size, prefix);
        }
    }

    /// <summary>
    /// Write an array of values of the specified unmanaged type to the data destination.
    /// </summary>
    /// <typeparam name="T">The unmanaged type of array elements.</typeparam>
    /// <param name="enumerable">The array of values to write.</param>
    /// <param name="size">The size of the value to write.</param>
    /// <param name="prefix">Whether or not to prefix the value with its length.</param>
    /// <exception cref="NotSupportedException">Thrown when the specified type is not supported.</exception>
    protected void Write<T>(IEnumerable<T> enumerable, int? size = default, bool? prefix = false)
    {
        foreach (var item in enumerable)
        {
            Write(item, size, prefix);
        }
    }

    /// <summary>
    /// Write bytes.
    /// </summary>
    /// <param name="value">The bytes to write.</param>
    protected void Write(byte[] value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write bytes.
    /// </summary>
    /// <param name="value">The bytes to write.</param>
    /// <param name="length">The number of bytes to write.</param>
    protected void Write(byte[] value, int length)
    {
        _writer.Write(value);

        for (var i = 0; i < length - value.Length; i++)
        {
            _writer.Write((byte) 0);
        }
    }

    /// <summary>
    /// Write a <see cref="byte"/>.
    /// </summary>
    /// <param name="value">The <see cref="byte"/> to write.</param>
    protected void Write(byte value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="value">The <see cref="sbyte"/> to write.</param>
    protected void Write(sbyte value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="char"/>.
    /// </summary>
    /// <param name="value">The <see cref="char"/> to write.</param>
    protected void Write(char value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="bool"/>.
    /// </summary>
    /// <param name="value">The <see cref="bool"/> to write.</param>
    protected void Write(bool value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="ushort"/>.
    /// </summary>
    /// <param name="value">The <see cref="ushort"/> to write.</param>
    protected void Write(ushort value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="short"/>.
    /// </summary>
    /// <param name="value">The <see cref="short"/> to write.</param>
    protected void Write(short value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="uint"/>.
    /// </summary>
    /// <param name="value">The <see cref="uint"/> to write.</param>
    protected void Write(uint value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write an <see cref="int"/>.
    /// </summary>
    /// <param name="value">The <see cref="int"/> to write.</param>
    protected void Write(int value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="float"/>.
    /// </summary>
    /// <param name="value">The <see cref="float"/> to write.</param>
    protected void Write(float value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The <see cref="ulong"/> to write.</param>
    protected void Write(ulong value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The <see cref="long"/> to write.</param>
    protected void Write(long value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="decimal"/>.
    /// </summary>
    /// <param name="value">The <see cref="decimal"/> to write.</param>
    protected void Write(decimal value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="double"/>.
    /// </summary>
    /// <param name="value">The <see cref="double"/> to write.</param>
    protected void Write(double value)
    {
        _writer.Write(value);
    }

    /// <summary>
    /// Write a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to write.</param>
    /// <param name="prefix">Whether or not to prefix the <see cref="string"/> with its length.</param>
    protected void Write(string value, bool prefix = false)
    {
        Write(value, value.Length, prefix);
    }

    /// <summary>
    /// Write a <see cref="string"/>.
    /// </summary>
    /// <param name="value">The <see cref="string"/> to write.</param>
    /// <param name="length">The length of the <see cref="string"/>.</param>
    /// <param name="prefix">Whether or not to prefix the <see cref="string"/> with its length.</param>
    protected void Write(string value, int length, bool prefix = false)
    {
        var bytes = Encoding.ASCII.GetBytes(value);

        if (prefix)
        {
            Write((byte) length);
        }

        Write(bytes);

        for (var i = 0; i < length - bytes.Length; i++)
        {
            Write((byte) 0);
        }
    }
}