// Copyright © Spatial Corporation. All rights reserved.

import { Neuron, Snapshot, Synapse } from "..";

/**
 * Configurable options for a neural network stream.
 */
export type StreamOptions = {
  /**
   * Called when initial hydration of the neural network is completed.
   */
  ready?: () => void;

  /**
   * Called when the neural network is updated.
   * @param snapshot A snapshot of the neural network.
   */
  updated?: (snapshot: Snapshot) => void;

  /**
   * Configurable options for neural behavior in the stream.
   */
  neurons?: NeuronStreamOptions;

  /**
   * Configurable options for synaptic behavior in the stream.
   */
  synapses?: SynapseStreamOptions;

  /**
   * Called when an error occurs during the stream.
   * @param error The {@link Error} that occurred.
   */
  error?: (error: unknown) => void;
};

/**
 * Configurable options for neural behavior in a neural network stream.
 */
export type NeuronStreamOptions = {
  /**
   * Called when a {@link Neuron} is added to the network.
   * @param neuron The {@link Neuron} that was added.
   */
  added?: (neuron: Neuron) => void;

  /**
   * Called when a {@link Neuron} is updated.
   * @param neuron The {@link Neuron} that was updated.
   */
  updated?: (neuron: Neuron) => void;

  /**
   * Called when a {@link Neuron} is removed from the network.
   * @param neuron The {@link Neuron} that was removed.
   */
  removed?: (neuron: Neuron) => void;
};

/**
 * Configurable options for the synaptic behavior in a neural network stream.
 */
export type SynapseStreamOptions = {
  /**
   * Called when a {@link Synapse} is added to the network.
   * @param synapse The {@link Synapse} that was added.
   */
  added?: (synapse: Synapse) => void;

  /**
   * Called when a {@link Synapse} is updated.
   * @param synapse The {@link Synapse} that was updated.
   */
  updated?: (synapse: Synapse) => void;

  /**
   * Called when a {@link Synapse} is removed from the network.
   * @param synapse The {@link Synapse} that was removed.
   */
  removed?: (synapse: Synapse) => void;
};
