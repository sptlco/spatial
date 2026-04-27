// Copyright © Spatial Corporation. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace Spatial.Cloud.Data.Scopes;

/// <summary>
/// Constant authorization scopes used throughout the system.
/// Scopes control access to different resources and operations.  
/// </summary>
public partial class Scope
{
    /// <summary>
    /// Scopes related to accounts.
    /// </summary>
    public class Accounts
    {
        /// <summary>
        /// Create an account.
        /// </summary>
        [Metadata("person_add", "Create an account.")]
        public const string Create = "accounts.create";

        /// <summary>
        /// Update an account.
        /// </summary>
        [Metadata("person_edit", "Update an account.")]
        public const string Update = "accounts.update";

        /// <summary>
        /// Delete an account.
        /// </summary>
        [Metadata("person_remove", "Delete an account.")]
        public const string Delete = "accounts.delete";
    }

    /// <summary>
    /// Scopes related to assets.
    /// </summary>
    public class Assets
    {
        /// <summary>
        /// Get a list of assets.
        /// </summary>
        [Metadata("Get a list of assets.")]
        public const string List = "assets.list";
    }

    /// <summary>
    /// Scopes related to assignments.
    /// </summary>
    public class Assignments
    {
        /// <summary>
        /// Get a list of assignments.
        /// </summary>
        [Metadata("Get a list of assignments.")]
        public const string List = "assignments.list";

        /// <summary>
        /// Patch role assignments.
        /// </summary>
        [Metadata("Patch role assignments.")]
        public const string Patch = "assignments.patch";
    }

    /// <summary>
    /// Scopes related to the neural network.
    /// </summary>
    public class Brain
    {
        /// <summary>
        /// Get a snapshot of the brain.
        /// </summary>
        [Metadata("Get a snapshot of the neural network.")]
        public const string Read = "brain.read";

        /// <summary>
        /// Stream live network state.
        /// </summary>
        [Metadata("Stream live neural network state.")]
        public const string Stream = "brain.stream";

        /// <summary>
        /// Stimulate a neuron.
        /// </summary>
        [Metadata("Stimulate a neuron.")]
        public const string Stimulate_Neuron = "brain.neurons.stimulate";

        /// <summary>
        /// Add a neuron to the neural network.
        /// </summary>
        [Metadata("Add a neuron to the neural network.")]
        public const string Add_Neuron = "brain.neurons.add";

        /// <summary>
        /// Remove a neuron from the neural network.
        /// </summary>
        [Metadata("Remove a neuron from the neural network.")]
        public const string Remove_Neuron = "brain.neurons.remove";

        /// <summary>
        /// Update a neuron.
        /// </summary>
        [Metadata("Update a neuron.")]
        public const string Update_Neuron = "brain.neurons.update";

        /// <summary>
        /// Add a synapse to the neural network.
        /// </summary>
        [Metadata("Add a synapse to the neural network.")]
        public const string Add_Synapse = "brain.synapses.add";

        /// <summary>
        /// Remove a synapse from the neural network.
        /// </summary>
        [Metadata("Remove a synapse from the neural network.")]
        public const string Remove_Synapse = "brain.synapses.remove";

        /// <summary>
        /// Update a synapse.
        /// </summary>
        [Metadata("Update a synapse.")]
        public const string Update_Synapse = "brain.synapses.update";
    }

    /// <summary>
    /// Scopes related to metric data.
    /// </summary>
    public class Metrics
    {
        /// <summary>
        /// Read metric data points.
        /// </summary>
        [Metadata("Read metric data points.")]
        public const string Read = "metrics.read";
    }

    /// <summary>
    /// Scopes related to permissions.
    /// </summary>
    public class Permissions
    {
        /// <summary>
        /// Update permissions.
        /// </summary>
        [Metadata("Update permissions.")]
        public const string Update = "permissions.update";

        /// <summary>
        /// Get a list of permissions.
        /// </summary>
        [Metadata("Get a list of permissions.")]
        public const string List = "permissions.list";
    }

    /// <summary>
    /// Scopes related to roles.
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// Create a role.
        /// </summary>
        [Metadata("group_add", "Create a role.")]
        public const string Create = "roles.create";

        /// <summary>
        /// Update a role.
        /// </summary>
        [Metadata("person_edit", "Update a role.")]
        public const string Update = "roles.update";

        /// <summary>
        /// Get a list of roles.
        /// </summary>
        [Metadata("Get a list of roles.")]
        public const string List = "roles.list";

        /// <summary>
        /// Delete a role.
        /// </summary>
        [Metadata("person_remove", "Delete a role.")]
        public const string Delete = "roles.delete";
    }

    /// <summary>
    /// Scopes related to scopes themselves.
    /// </summary>
    public class Scopes
    {
        /// <summary>
        /// Get a list of scopes.
        /// </summary>
        [Metadata("Get a list of scopes.")]
        public const string List = "scopes.list";
    }

    /// <summary>
    /// Scopes related to shipments.
    /// </summary>
    public class Shipments
    {
        /// <summary>
        /// Create a new shipment.
        /// </summary>
        [Metadata("Create a new shipment.")]
        public const string Create = "shipments.create";

        /// <summary>
        /// Find a <see cref="Shipment"/>.
        /// </summary>
        [Metadata("Find a shipment.")]
        public const string Find = "shipments.find";

        /// <summary>
        /// Get a list of shipments.
        /// </summary>
        [Metadata("Get a list of shipments.")]
        public const string List = "shipments.list";

        /// <summary>
        /// Update a parcel.
        /// </summary>
        [Metadata("Update a parcel.")]
        public const string Update_Parcel = "shipments.parcels.update";

        /// <summary>
        /// Delete a parcel.
        /// </summary>
        [Metadata("Delete a parcel.")]
        public const string Delete_Parcel = "shipments.parcels.delete";

        /// <summary>
        /// Delete a shipment.
        /// </summary>
        [Metadata("Delete a shipment")]
        public const string Delete = "shipments.delete";
    }

    /// <summary>
    /// Scopes related to users.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Get a list of users.
        /// </summary>
        [Metadata("Get a list of users.")]
        public const string List = "users.list";
    }
}