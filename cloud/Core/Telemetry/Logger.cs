// Copyright © Spatial Corporation. All rights reserved.

using Serilog;

namespace Spatial.Telemetry;

/// <summary>
/// A static wrapper for Serilog logging with standardized log levels.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Log a trace message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void TRACE(string template)
    {
        Log.Verbose(template);
    }

    /// <summary>
    /// Log a trace message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void TRACE(string template, params object[] properties)
    {
        Log.Verbose(template, properties);
    }

    /// <summary>
    /// Log a trace message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void TRACE(Exception? exception, string template, params object[] properties)
    {
        Log.Verbose(exception, template, properties);
    }

    /// <summary>
    /// Log a trace message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void TRACE(Error? error, string template, params object[] properties)
    {
        Log.Verbose(error?.ToFault(), template, properties);
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void DEBUG(string template)
    {
        Log.Debug(template);
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void DEBUG(string template, params object[] properties)
    {
        Log.Debug(template, properties);
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void DEBUG(Exception? exception, string template, params object[] properties)
    {
        Log.Debug(exception, template, properties);
    }

    /// <summary>
    /// Log a debug message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void DEBUG(Error? error, string template, params object[] properties)
    {
        Log.Debug(error?.ToFault(), template, properties);
    }

    /// <summary>
    /// Log an information message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void INFO(string template)
    {
        Log.Information(template);
    }

    /// <summary>
    /// Log an information message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void INFO(string template, params object[] properties)
    {
        Log.Information(template, properties);
    }

    /// <summary>
    /// Log an information message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void INFO(Exception? exception, string template, params object[] properties)
    {
        Log.Information(exception, template, properties);
    }

    /// <summary>
    /// Log an information message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void INFO(Error? error, string template, params object[] properties)
    {
        Log.Information(error?.ToFault(), template, properties);
    }

    /// <summary>
    /// Log a warning message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void WARN(string template)
    {
        Log.Warning(template);
    }

    /// <summary>
    /// Log a warning message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void WARN(string template, params object[] properties)
    {
        Log.Warning(template, properties);
    }

    /// <summary>
    /// Log a warning message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void WARN(Exception? exception, string template, params object[] properties)
    {
        Log.Warning(exception, template, properties);
    }

    /// <summary>
    /// Log a warning message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void WARN(Error? error, string template, params object[] properties)
    {
        Log.Warning(error?.ToFault(), template, properties);
    }

    /// <summary>
    /// Log an error message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void ERROR(string template)
    {
        Log.Error(template);
    }

    /// <summary>
    /// Log an error message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void ERROR(string template, params object[] properties)
    {
        Log.Error(template, properties);
    }

    /// <summary>
    /// Log an error message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void ERROR(Exception? exception, string template, params object[] properties)
    {
        Log.Error(exception, template, properties);
    }

    /// <summary>
    /// Log an error message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void ERROR(Error? error, string template, params object[] properties)
    {
        Log.Error(error?.ToFault(), template, properties);
    }

    /// <summary>
    /// Log a fatal message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    public static void FATAL(string template)
    {
        Log.Fatal(template);
    }

    /// <summary>
    /// Log a fatal message.
    /// </summary>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void FATAL(string template, params object[] properties)
    {
        Log.Fatal(template, properties);
    }

    /// <summary>
    /// Log a fatal message.
    /// </summary>
    /// <param name="exception">An <see cref="Exception"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void FATAL(Exception? exception, string template, params object[] properties)
    {
        Log.Fatal(exception, template, properties);
    }

    /// <summary>
    /// Log a fatal message.
    /// </summary>
    /// <param name="error">An <see cref="Error"/> that occurred.</param>
    /// <param name="template">The message's template.</param>
    /// <param name="properties">Contextual properties for the log message.</param>
    public static void FATAL(Error? error, string template, params object[] properties)
    {
        Log.Fatal(error?.ToFault(), template, properties);
    }
}