// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// The occurrence of unexpected behavior at runtime.
/// </summary>
public abstract class Error
{
    /// <summary>
    /// Create a new <see cref="Error"/>.
    /// </summary>
    /// <param name="message">A message describing what went wrong.</param>
    /// <param name="status">The status of a request after the <see cref="Error"/> occurred.</param>
    public Error(string message, int status)
    {
        Time = Spatial.Time.Now;
        Status = status;
        Code = GetLineage();
        Message = message;
    }

    /// <summary>
    /// The <see cref="Spatial.Time"/> the <see cref="Error"/> occurred.
    /// </summary>
    public double Time { get; }

    /// <summary>
    /// The status of a request after the <see cref="Error"/> occurred.
    /// </summary>
    public int Status { get; }

    /// <summary>
    /// A classifying <see cref="Error"/> code.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// A message describing what went wrong.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Create a new <see cref="Fault"/>.
    /// </summary>
    /// <param name="error">An <see cref="Error"/>.</param>
    public static implicit operator Fault(Error error) => error.ToFault();

    /// <summary>
    /// Convert the <see cref="Error"/> to a <see cref="Fault"/>.
    /// </summary>
    /// <returns>A <see cref="Fault"/>.</returns>
    public Fault ToFault()
    {
        return new Fault(this);
    }

    private string GetLineage()
    {
        var types = new Stack<string>();
        var type = GetType();

        while (type != null && type != typeof(Error))
        {
            types.Push(type.Name);
            type = type.BaseType;
        }

        return string.Join("/", types);
    }
}