// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../..";

/**
 * A weighted connection between two neurons.
 */
export type Synapse = Schema<{
  /**
   * The neuron the {@link Synapse} extends from.
   */
  from: string;

  /**
   * The neuron the {@link Synapse} extends to.
   */
  to: string;

  /**
   * The strength of the {@link Synapse}.
   */
  strength: number;
}>;
