// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// A progressive spinner for the <see cref="Console"/>.
/// </summary>
public sealed class Spinner : IDisposable
{
    private static readonly char[] _frames = ['|', '/', '-', '\\'];
    private int _frame;
    private readonly object _lock = new();
    private int _startLine = -1;
    private int _lastLineCount;
    private bool _disposed;

    /// <summary>
    /// Spin the <see cref="Spinner"/>.
    /// </summary>
    /// <param name="label">An optional label.</param>
    public void Spin(string? label = null)
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            InitializeStartLine();
            
            Console.CursorVisible = false;

            ClearPreviousLines();
            WriteFrame(label);
            UpdateLineTracking();
        }
    }

    /// <summary>
    /// Dispose of the <see cref="Spinner"/>.
    /// </summary>
    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }
            
            ClearAllSpinnerLines();
            Console.CursorVisible = true;
            _disposed = true;
        }
    }

    private void InitializeStartLine()
    {
        if (_startLine == -1)
        {
            _startLine = Console.CursorTop;
        }
    }

    private void ClearPreviousLines()
    {
        if (_lastLineCount == 0)
        {
            return;
        }

        Console.SetCursorPosition(0, _startLine);

        for (var i = 0; i < _lastLineCount; i++)
        {
            Console.Write(new string(' ', Console.WindowWidth));

            if (_startLine + i + 1 < Console.BufferHeight)
            {
                Console.SetCursorPosition(0, _startLine + i + 1);
            }
        }

        Console.SetCursorPosition(0, _startLine);
    }

    private void WriteFrame(string? label)
    {
        Console.Write($"{_frames[_frame]}{(!string.IsNullOrEmpty(label) ? $" {label}" : "")}");
        _frame = (_frame + 1) % _frames.Length;
    }

    private void UpdateLineTracking()
    {
        _lastLineCount = Console.CursorTop - _startLine + (Console.CursorLeft > 0 ? 1 : 0);
    }

    private void ClearAllSpinnerLines()
    {
        if (_startLine == -1)
        {
            return;
        }

        var top = Console.CursorTop;

        Console.SetCursorPosition(0, _startLine);

        for (var i = 0; i < _lastLineCount; i++)
        {
            Console.Write(new string(' ', Console.WindowWidth));

            if (_startLine + i + 1 < Console.BufferHeight)
            {
                Console.SetCursorPosition(0, _startLine + i + 1);
            }
        }

        Console.SetCursorPosition(0, Math.Min(_startLine, top));
    }
}