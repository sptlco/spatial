// Copyright © Spatial Corporation. All rights reserved.

using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace Spatial.Networking;

/// <summary>
/// An object used to transmit data over the network.
/// </summary>
public abstract class ProtocolBuffer : IDisposable
{
    private byte[] _buffer;
    private int _position;
    private int _length;
    private bool _disposed;

    /// <summary>
    /// Create a new <see cref="ProtocolBuffer"/>.
    /// </summary>
    public ProtocolBuffer()
    {
        _buffer = ArrayPool<byte>.Shared.Rent(256);
        _position = 0;
        _length = 0;
    }

    /// <summary>
    /// The valid, written data of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    internal ReadOnlySpan<byte> Data => _buffer.AsSpan(0, _length);

    /// <summary>
    /// The size of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public long Size => _length;

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

        buffer.EnsureCapacity(data.Length);
        data.CopyTo(buffer._buffer);

        buffer._length = data.Length;
        buffer._position = 0;

        buffer.Deserialize();

        return buffer;
    }

    /// <summary>
    /// Serialize the <see cref="ProtocolBuffer"/>.
    /// </summary>
    /// <param name="clear">Whether or not to clear the <see cref="ProtocolBuffer"/>.</param>
    public void Serialize(bool clear)
    {
        if (clear)
        {
            _position = 0;
            _length = 0;
        }

        Serialize();
    }

    /// <summary>
    /// Serialize the protocol buffer.
    /// </summary>
    public virtual void Serialize() { }

    /// <summary>
    /// Deserialize the protocol buffer.
    /// </summary>
    public virtual void Deserialize() { }

    /// <summary>
    /// Convert the protocol buffer to an array.
    /// </summary>
    /// <returns>An array of bytes.</returns>
    public byte[] ToArray()
    {
        return Data.ToArray();
    }

    /// <summary>
    /// Dispose of the <see cref="ProtocolBuffer"/>.
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        ArrayPool<byte>.Shared.Return(_buffer);
        _disposed = true;

        GC.SuppressFinalize(this);
    }

    private void EnsureCapacity(int additional)
    {
        var required = _position + additional;

        if (required <= _buffer.Length)
        {
            return;
        }

        var size = _buffer.Length * 2;

        while (size < required)
        {
            size *= 2;
        }

        var buffer = ArrayPool<byte>.Shared.Rent(size);

        _buffer.AsSpan(0, _length).CopyTo(buffer);

        ArrayPool<byte>.Shared.Return(_buffer);

        _buffer = buffer;
    }

    /// <summary>
    /// Temporarily hands this buffer's storage to a nested <see cref="ProtocolBuffer"/> so it can
    /// deserialize directly out of the parent's bytes at the parent's current position, then folds
    /// the resulting position back into the parent.
    /// </summary>
    private void ReadNested(ProtocolBuffer nested)
    {
        var buffer = nested._buffer;
        var position = nested._position;
        var length = nested._length;

        nested._buffer = _buffer;
        nested._position = _position;
        nested._length = _length;

        nested.Deserialize();

        _position = nested._position;

        nested._buffer = buffer;
        nested._position = position;
        nested._length = length;
    }

    /// <summary>
    /// Temporarily hands this buffer's storage to a nested <see cref="ProtocolBuffer"/> so it can
    /// serialize directly into the parent's bytes at the parent's current position (growing the
    /// shared buffer if needed), then folds the resulting buffer/position back into the parent.
    /// </summary>
    private void WriteNested(ProtocolBuffer nested)
    {
        var buffer = nested._buffer;
        var position = nested._position;
        var length = nested._length;

        nested._buffer = _buffer;
        nested._position = _position;
        nested._length = _length;

        nested.Serialize(false);

        _buffer = nested._buffer;
        _position = nested._position;
        _length = nested._length;

        nested._buffer = buffer;
        nested._position = position;
        nested._length = length;
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
                ReadNested(buffer);
                return (T) (object) buffer;
            case Type t when t == typeof(byte):
                { var v = ReadByte(); return Unsafe.As<byte, T>(ref v); }
            case Type t when t == typeof(char):
                { var v = ReadChar(); return Unsafe.As<char, T>(ref v); }
            case Type t when t == typeof(short):
                { var v = ReadInt16(); return Unsafe.As<short, T>(ref v); }
            case Type t when t == typeof(ushort):
                { var v = ReadUInt16(); return Unsafe.As<ushort, T>(ref v); }
            case Type t when t == typeof(int):
                { var v = ReadInt32(); return Unsafe.As<int, T>(ref v); }
            case Type t when t == typeof(uint):
                { var v = ReadUInt32(); return Unsafe.As<uint, T>(ref v); }
            case Type t when t == typeof(long):
                { var v = ReadInt64(); return Unsafe.As<long, T>(ref v); }
            case Type t when t == typeof(ulong):
                { var v = ReadUInt64(); return Unsafe.As<ulong, T>(ref v); }
            case Type t when t == typeof(float):
                { var v = ReadSingle(); return Unsafe.As<float, T>(ref v); }
            case Type t when t == typeof(double):
                { var v = ReadDouble(); return Unsafe.As<double, T>(ref v); }
            case Type t when t == typeof(string):
                return (T) (object) ReadString(size ?? (_length - _position));
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
        var result = _buffer.AsSpan(_position, _length - _position).ToArray();
        _position = _length;
        return result;
    }

    /// <summary>
    /// Read bytes.
    /// </summary>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>An array of bytes.</returns>
    protected byte[] ReadBytes(int count)
    {
        var result = _buffer.AsSpan(_position, count).ToArray();
        _position += count;
        return result;
    }

    /// <summary>
    /// Read a <see cref="byte"/>.
    /// </summary>
    /// <returns>A <see cref="byte"/>.</returns>
    protected byte ReadByte()
    {
        var value = _buffer[_position];
        _position += sizeof(byte);
        return value;
    }

    /// <summary>
    /// Read a <see cref="sbyte"/>.
    /// </summary>
    /// <returns>A <see cref="sbyte"/>.</returns>
    protected sbyte ReadSByte()
    {
        var value = (sbyte) _buffer[_position];
        _position += sizeof(sbyte);
        return value;
    }

    /// <summary>
    /// Read a <see cref="char"/>.
    /// </summary>
    /// <returns>A <see cref="char"/>.</returns>
    /// <remarks>
    /// Reads a single byte, unlike <see cref="BinaryReader.ReadChar"/> which decodes via the
    /// stream's text encoding and can consume a variable number of bytes. For this protocol's
    /// single-byte ASCII fields the behavior is identical; it would differ only for non-ASCII input.
    /// </remarks>
    protected char ReadChar()
    {
        var value = (char) _buffer[_position];
        _position += sizeof(byte);
        return value;
    }

    /// <summary>
    /// Read a <see cref="bool"/>.
    /// </summary>
    /// <returns>A <see cref="bool"/>.</returns>
    protected bool ReadBoolean()
    {
        var value = _buffer[_position] != 0;
        _position += sizeof(byte);
        return value;
    }

    /// <summary>
    /// Read a <see cref="ushort"/>.
    /// </summary>
    /// <returns>A <see cref="ushort"/>.</returns>
    protected ushort ReadUInt16()
    {
        var value = BinaryPrimitives.ReadUInt16LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(ushort);
        return value;
    }

    /// <summary>
    /// Read a <see cref="short"/>.
    /// </summary>
    /// <returns>A <see cref="short"/>.</returns>
    protected short ReadInt16()
    {
        var value = BinaryPrimitives.ReadInt16LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(short);
        return value;
    }

    /// <summary>
    /// Read a <see cref="uint"/>.
    /// </summary>
    /// <returns>A <see cref="uint"/>.</returns>
    protected uint ReadUInt32()
    {
        var value = BinaryPrimitives.ReadUInt32LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(uint);
        return value;
    }

    /// <summary>
    /// Read an <see cref="int"/>.
    /// </summary>
    /// <returns>An <see cref="int"/>.</returns>
    protected int ReadInt32()
    {
        var value = BinaryPrimitives.ReadInt32LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(int);
        return value;
    }

    /// <summary>
    /// Read a <see cref="float"/>.
    /// </summary>
    /// <returns>A <see cref="float"/>.</returns>
    protected float ReadSingle()
    {
        var value = BinaryPrimitives.ReadSingleLittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(float);
        return value;
    }

    /// <summary>
    /// Read a <see cref="ulong"/>.
    /// </summary>
    /// <returns>A <see cref="ulong"/>.</returns>
    protected ulong ReadUInt64()
    {
        var value = BinaryPrimitives.ReadUInt64LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(ulong);
        return value;
    }

    /// <summary>
    /// Read a <see cref="long"/>.
    /// </summary>
    /// <returns>A <see cref="long"/>.</returns>
    protected long ReadInt64()
    {
        var value = BinaryPrimitives.ReadInt64LittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(long);
        return value;
    }

    /// <summary>
    /// Read a <see cref="decimal"/>.
    /// </summary>
    /// <returns>A <see cref="decimal"/>.</returns>
    protected decimal ReadDecimal()
    {
        // BinaryPrimitives has no decimal support; BinaryWriter.Write(decimal) lays it out
        // as four little-endian int32 components ([lo, mid, hi, flags]), so mirror that here.

        Span<int> bits = stackalloc int[4];

        for (var i = 0; i < 4; i++)
        {
            bits[i] = ReadInt32();
        }

        return new decimal(bits);
    }

    /// <summary>
    /// Read a <see cref="double"/>.
    /// </summary>
    /// <returns>A <see cref="double"/>.</returns>
    protected double ReadDouble()
    {
        var value = BinaryPrimitives.ReadDoubleLittleEndian(_buffer.AsSpan(_position));
        _position += sizeof(double);
        return value;
    }

    /// <summary>
    /// Read a <see cref="string"/>.
    /// </summary>
    /// <param name="length">The length of the <see cref="string"/>.</param>
    /// <returns>A <see cref="string"/>.</returns>
    protected string ReadString(int length)
    {
        var span = _buffer.AsSpan(_position, length);

        _position += length;

        var count = 0;

        if (span[length - 1] != 0)
        {
            count = length;
        }
        else
        {
            while (count < span.Length && span[count] != 0)
            {
                count++;
            }
        }

        return count > 0 ? Encoding.ASCII.GetString(span[..count]) : string.Empty;
    }

    /// <summary>
    /// Fill the <see cref="ProtocolBuffer"/> with an arbitrary value.
    /// </summary>
    /// <param name="count">The number of bytes to fill.</param>
    /// <param name="value">The value to fill the bytes with.</param>
    protected void Fill(int count, byte value)
    {
        EnsureCapacity(count);

        _buffer.AsSpan(_position, count).Fill(value);

        _position += count;
        _length = Math.Max(_length, _position);
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
                WriteNested((ProtocolBuffer) (object) value);
                break;
            case Type t when t == typeof(byte):
                Write(Unsafe.As<T, byte>(ref value));
                break;
            case Type t when t == typeof(char):
                Write(Unsafe.As<T, char>(ref value));
                break;
            case Type t when t == typeof(bool):
                Write(Unsafe.As<T, bool>(ref value));
                break;
            case Type t when t == typeof(short):
                Write(Unsafe.As<T, short>(ref value));
                break;
            case Type t when t == typeof(ushort):
                Write(Unsafe.As<T, ushort>(ref value));
                break;
            case Type t when t == typeof(int):
                Write(Unsafe.As<T, int>(ref value));
                break;
            case Type t when t == typeof(uint):
                Write(Unsafe.As<T, uint>(ref value));
                break;
            case Type t when t == typeof(long):
                Write(Unsafe.As<T, long>(ref value));
                break;
            case Type t when t == typeof(ulong):
                Write(Unsafe.As<T, ulong>(ref value));
                break;
            case Type t when t == typeof(float):
                Write(Unsafe.As<T, float>(ref value));
                break;
            case Type t when t == typeof(double):
                Write(Unsafe.As<T, double>(ref value));
                break;
            case Type t when t == typeof(string):
                var str = (string) (object) value;
                Write(str, size ?? str.Length, prefix ?? false);
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
        EnsureCapacity(value.Length);

        value.CopyTo(_buffer.AsSpan(_position));

        _position += value.Length;
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write bytes.
    /// </summary>
    /// <param name="value">The bytes to write.</param>
    /// <param name="length">The number of bytes to write.</param>
    protected void Write(byte[] value, int length)
    {
        EnsureCapacity(length);

        value.CopyTo(_buffer.AsSpan(_position));

        var padding = length - value.Length;

        if (padding > 0)
        {
            _buffer.AsSpan(_position + value.Length, padding).Clear();
        }

        _position += length;
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="byte"/>.
    /// </summary>
    /// <param name="value">The <see cref="byte"/> to write.</param>
    protected void Write(byte value)
    {
        EnsureCapacity(sizeof(byte));

        _buffer[_position] = value;
        _position += sizeof(byte);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="value">The <see cref="sbyte"/> to write.</param>
    protected void Write(sbyte value)
    {
        Write((byte) value);
    }

    /// <summary>
    /// Write a <see cref="char"/>.
    /// </summary>
    /// <param name="value">The <see cref="char"/> to write.</param>
    /// <remarks>
    /// Writes a single byte, matching <see cref="ReadChar"/> above; see its remarks for why this
    /// differs from <see cref="BinaryWriter.Write(char)"/>.
    /// </remarks>
    protected void Write(char value)
    {
        Write((byte) value);
    }

    /// <summary>
    /// Write a <see cref="bool"/>.
    /// </summary>
    /// <param name="value">The <see cref="bool"/> to write.</param>
    protected void Write(bool value)
    {
        Write((byte) (value ? 1 : 0));
    }

    /// <summary>
    /// Write a <see cref="ushort"/>.
    /// </summary>
    /// <param name="value">The <see cref="ushort"/> to write.</param>
    protected void Write(ushort value)
    {
        EnsureCapacity(sizeof(ushort));
        BinaryPrimitives.WriteUInt16LittleEndian(_buffer.AsSpan(_position), value);
        _position += sizeof(ushort);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="short"/>.
    /// </summary>
    /// <param name="value">The <see cref="short"/> to write.</param>
    protected void Write(short value)
    {
        EnsureCapacity(sizeof(short));

        BinaryPrimitives.WriteInt16LittleEndian(_buffer.AsSpan(_position), value);

        _position += sizeof(short);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="uint"/>.
    /// </summary>
    /// <param name="value">The <see cref="uint"/> to write.</param>
    protected void Write(uint value)
    {
        EnsureCapacity(sizeof(uint));
        BinaryPrimitives.WriteUInt32LittleEndian(_buffer.AsSpan(_position), value);
        _position += sizeof(uint);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write an <see cref="int"/>.
    /// </summary>
    /// <param name="value">The <see cref="int"/> to write.</param>
    protected void Write(int value)
    {
        EnsureCapacity(sizeof(int));

        BinaryPrimitives.WriteInt32LittleEndian(_buffer.AsSpan(_position), value);

        _position += sizeof(int);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="float"/>.
    /// </summary>
    /// <param name="value">The <see cref="float"/> to write.</param>
    protected void Write(float value)
    {
        EnsureCapacity(sizeof(float));

        BinaryPrimitives.WriteSingleLittleEndian(_buffer.AsSpan(_position), value);
        
        _position += sizeof(float);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="ulong"/>.
    /// </summary>
    /// <param name="value">The <see cref="ulong"/> to write.</param>
    protected void Write(ulong value)
    {
        EnsureCapacity(sizeof(ulong));
       
        BinaryPrimitives.WriteUInt64LittleEndian(_buffer.AsSpan(_position), value);
        
        _position += sizeof(ulong);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="long"/>.
    /// </summary>
    /// <param name="value">The <see cref="long"/> to write.</param>
    protected void Write(long value)
    {
        EnsureCapacity(sizeof(long));
        
        BinaryPrimitives.WriteInt64LittleEndian(_buffer.AsSpan(_position), value);
        
        _position += sizeof(long);
        _length = Math.Max(_length, _position);
    }

    /// <summary>
    /// Write a <see cref="decimal"/>.
    /// </summary>
    /// <param name="value">The <see cref="decimal"/> to write.</param>
    protected void Write(decimal value)
    {
        Span<int> bits = stackalloc int[4];

        decimal.GetBits(value, bits);

        foreach (var part in bits)
        {
            Write(part);
        }
    }

    /// <summary>
    /// Write a <see cref="double"/>.
    /// </summary>
    /// <param name="value">The <see cref="double"/> to write.</param>
    protected void Write(double value)
    {
        EnsureCapacity(sizeof(double));
        
        BinaryPrimitives.WriteDoubleLittleEndian(_buffer.AsSpan(_position), value);
        
        _position += sizeof(double);
        _length = Math.Max(_length, _position);
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
        var byteCount = Encoding.ASCII.GetByteCount(value);

        EnsureCapacity((prefix ? sizeof(byte) : 0) + Math.Max(length, byteCount));

        if (prefix)
        {
            Write((byte) length);
        }

        var written = Encoding.ASCII.GetBytes(value, _buffer.AsSpan(_position));

        _position += written;
        _length = Math.Max(_length, _position);

        var padding = length - written;

        if (padding > 0)
        {
            _buffer.AsSpan(_position, padding).Clear();
            _position += padding;
            _length = Math.Max(_length, _position);
        }
    }
}