// Copyright © Spatial Corporation. All rights reserved.

using OpenAI;
using OpenAI.Chat;
using Spatial.Cloud.Contracts;
using Spatial.Cloud.Helpers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Spatial.Cloud;

/// <summary>
/// Helper methods for OpenAI.
/// </summary>
public static class OpenAI
{
    private static OpenAIClient? _client;

    /// <summary>
    /// The global <see cref="OpenAIClient"/>.
    /// </summary>
    public static OpenAIClient Client => _client ??= new OpenAIClient(ServerConfiguration.Current.OpenAI.ApiKey);

    /// <summary>
    /// Generate a response of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of response to generate.</typeparam>
    /// <param name="messages">Prompt messages.</param>
    /// <returns>A response of type <typeparamref name="T"/>.</returns>
    public static async Task<T> GenerateAsync<T>(string instructions, object input) where T : Response
    {
        var completion = await Client.GetChatClient(Constants.GPT).CompleteChatAsync(
            messages: [
                new SystemChatMessage("You are a chat interface. Return a JSON object of the requested type, including the name of this model."),
                new UserChatMessage($"{instructions}\n\nInput:\n{JsonSerializer.Serialize(input)}")
            ],
            options: new ChatCompletionOptions {
                ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "Response",
                    jsonSchema: BinaryData.FromString(Json.Schema<T>()),
                    jsonSchemaFormatDescription: "A response from the chat interface.",
                    jsonSchemaIsStrict: true
                )
            });

        return JsonSerializer.Deserialize<T>(completion.Value.Content[0].Text, new JsonSerializerOptions {
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        })!;
    }

    /// <summary>
    /// A response from OpenAI.
    /// </summary>
    public abstract class Response
    {
        /// <summary>
        /// The name of the model that generated the response.
        /// </summary>
        public string Model { get; set; }
    }
}