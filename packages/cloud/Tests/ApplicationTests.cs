// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial;

/// <summary>
/// Tests for an <see cref="Application"/>.
/// </summary>
public class ApplicationTests
{
    /// <summary>
    /// Run an <see cref="Application"/>.
    /// </summary>
    [Fact]
    [Trait("Category", "Unit")]
    public async Task TestRunAsync()
    {
        await Application.RunAsync<App>(new CancellationTokenSource(TimeSpan.FromSeconds(1)).Token);

        Assert.True(File.Exists("TestRunAsync.txt"));
        Assert.Equal("Success", File.ReadAllText("TestRunAsync.txt"));

        File.Delete("TestRunAsync.txt");
    }

    private class App : Application
    {
        /// <summary>
        /// Start the <see cref="App"/>.
        /// </summary>
        public override void Start()
        {
            File.WriteAllText("TestRunAsync.txt", "Success");
        }
    }
}
