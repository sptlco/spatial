// Copyright © Spatial Corporation. All rights reserved.

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Execute a <see cref="Task"/> with retries.
    /// </summary>
    /// <param name="task">The <see cref="Task"/> to execute.</param>
    /// <param name="retries">The number of times to retry the <see cref="Task"/>.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task<T> Retry<T>(this Task<T> task, int retries) => Retry(task, retries, (attempt, _) => 500 * attempt);

    /// <summary>
    /// Execute a <see cref="Task"/> with retries.
    /// </summary>
    /// <param name="task">The <see cref="Task"/> to execute.</param>
    /// <param name="retries">The number of times to retry the <see cref="Task"/>.</param>
    /// <param name="interval">A fixed interval to wait between attempts.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static Task<T> Retry<T>(this Task<T> task, int retries, int interval) => Retry(task, retries, (_, _) => interval);

    /// <summary>
    /// Execute a <see cref="Task"/> with retries.
    /// </summary>
    /// <param name="task">The <see cref="Task"/> to execute.</param>
    /// <param name="retries">The number of times to retry the <see cref="Task"/>.</param>
    /// <param name="interval">An interval factory.</param>
    /// <returns>The <see cref="Task"/>.</returns>
    public static async Task<T> Retry<T>(this Task<T> task, int retries, Func<int, int, int> interval)
    {
        var attempt = 0;

        while (true)
        {
            try
            {
                return await task;
            }
            catch when (attempt < retries - 1)
            {
                await Task.Delay(interval(attempt += 1, retries));
            }
        }
    }
}