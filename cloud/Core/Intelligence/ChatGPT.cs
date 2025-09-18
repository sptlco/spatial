// Copyright © Spatial Corporation. All rights reserved.

using OpenAI;
using OpenAI.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Intelligence;

/// <summary>
/// A means of interaction with ChatGPT.
/// </summary>
public static class ChatGPT
{
    private static OpenAIClient? _client;

    /// <summary>
    /// The global <see cref="OpenAIClient"/>.
    /// </summary>
    public static OpenAIClient Client => _client ??= new OpenAIClient(Configuration.Current.OpenAI.ApiKey);

    /// <summary>
    /// Generate a response of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of response to generate.</typeparam>
    /// <param name="instructions">Instructions to assist OpenAI in generating an output.</param>
    /// <param name="input">An arbitrary, contextual object.</param>
    /// <returns>A response of type <typeparamref name="T"/>.</returns>
    public static async Task<T> GenerateAsync<T>(string instructions, object input)
    {
        var completion = await Client.GetChatClient(Configuration.Current.OpenAI.Model).CompleteChatAsync(
            messages: [new UserChatMessage($"{instructions}\n\nInput:\n{JsonSerializer.Serialize(input)}")],
            options: new ChatCompletionOptions {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "Response",
                    jsonSchema: BinaryData.FromString(Json.Schema<T>()),
                    jsonSchemaFormatDescription: "A response from OpenAI.",
                    jsonSchemaIsStrict: true
                )
            });

        var response = completion.Value.Content[0].Text;

        INFO("{Model}: {Response}.", completion.Value.Model, response);

        return JsonSerializer.Deserialize<T>(response, new JsonSerializerOptions {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        })!;
    }
}