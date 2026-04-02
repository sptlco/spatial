// Copyright © Spatial Corporation. All rights reserved.

import { Neuron, Synapse } from "../..";

/**
 * Configurable options for a {@link Synapse} update.
 */
export type UpdateSynapseOptions = {
  /**
   * The {@link Neuron} the {@link Synapse} extends from.
   */
  from?: string;

  /**
   * The {@link Neuron} the {@link Synapse} extends to.
   */
  to?: string;

  /**
   * The current strength of the {@link Synapse}.
   */
  strength?: number;
};
