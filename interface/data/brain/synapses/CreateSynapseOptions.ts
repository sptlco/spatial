// Copyright © Spatial Corporation. All rights reserved.

import { CreateResourceOptions, Neuron, Synapse } from "../..";

/**
 * Configurable options for a new {@link Synapse}.
 */
export type CreateSynapseOptions = CreateResourceOptions & {
  /**
   * The {@link Neuron} the {@link Synapse} extends from.
   */
  from: string;

  /**
   * The {@link Neuron} the {@link Synapse} extends to.
   */
  to: string;

  /**
   * The current strength of the {@link Synapse}.
   */
  strength: number;
};
