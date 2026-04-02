// Copyright © Spatial Corporation. All rights reserved.

import { CreateResourceOptions, Neuron, NeuronType, Point3D } from "../..";

/**
 * Configurable options for a new {@link Neuron}.
 */
export type CreateNeuronOptions = CreateResourceOptions & {
  /**
   * The neuron's {@link NeuronType}.
   */
  type: NeuronType;

  /**
   * The neuron's group identification number.
   */
  group: number;

  /**
   * The channel the {@link Neuron} maps to.
   */
  channel: number;

  /**
   * The precise location of the {@link Neuron}.
   */
  position: Point3D;

  /**
   * The neuron's current activation level.
   */
  value: number;
};
