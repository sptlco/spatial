// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Intelligence;

/// <summary>
/// Configurable options for OpenAI.
/// </summary>
public class OpenAIConfiguration
{
    /// <summary>
    /// An OpenAI API key.
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// The model used for requests sent to OpenAI.
    /// </summary>
    public string Model { get; set; } = "gpt-5";
}