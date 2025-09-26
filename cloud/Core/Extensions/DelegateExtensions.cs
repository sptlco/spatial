// Copyright © Spatial Corporation. All rights reserved.

using System.Collections.Concurrent;
using System.Reflection;

namespace Spatial.Extensions;

/// <summary>
/// Extension methods for <see cref="Delegate"/>.
/// </summary>
public static class DelegateExtensions
{
    private static readonly ConcurrentDictionary<MethodInfo, string> _cache = new();
    
    /// <summary>
    /// Get a unique <see cref="Delegate"/> identifier.
    /// </summary>
    /// <param name="delegate">A <see cref="Delegate"/>.</param>
    /// <returns>The delegate's identifier.</returns>
    public static string GetOrCreateIdentifier(this Delegate @delegate)
    {
        return _cache.GetOrAdd(@delegate.Method, method => {
            var type = method.DeclaringType?.FullName ?? "<UnknownType>";
            var parameters = method.GetParameters();
            var returned = method.ReturnType.FullName ?? method.ReturnType.Name;

            return $"{returned} {type}.{method.Name}({string.Join(",", parameters.Select(p => p.ParameterType.FullName ?? p.ParameterType.Name))})";
        });
    }
}