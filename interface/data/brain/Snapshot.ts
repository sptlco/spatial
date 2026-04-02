// Copyright © Spatial Corporation. All rights reserved.

import { NeuronSnapshot, SynapseSnapshot } from "..";

/**
 * The current state of the neural network.
 */
export type Snapshot = {
  /**
   * The network's neurons.
   */
  neurons: NeuronSnapshot[];

  /**
   * The network's synapses.
   */
  synapses: SynapseSnapshot[];
};
