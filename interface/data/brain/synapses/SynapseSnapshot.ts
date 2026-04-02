// Copyright © Spatial Corporation. All rights reserved.

import { Neuron, Synapse } from "../..";

/**
 * An immutable point-in-time view of a {@link Synapse}.
 */
export type SynapseSnapshot = {
  /**
   * The synapse's unique identifier.
   */
  id: string;

  /**
   * The {@link Neuron} the {@link Synapse} extends from.
   */
  from: string;

  /**
   * The {@link Neuron} the {@link Synapse} extends to.
   */
  to: string;

  /**
   * The strength of the {@link Synapse}.
   */
  strength: number;
};
