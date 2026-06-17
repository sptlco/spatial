// Copyright © Spatial Corporation. All rights reserved.

using Microsoft.AspNetCore.Authorization;

namespace Spatial.Identity.Authorization;

/// <summary>
/// Requires a user to have a specified permission.
/// </summary>
public class PermissionRequirement : IAuthorizationRequirement { }