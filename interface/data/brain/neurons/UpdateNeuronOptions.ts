// Copyright © Spatial Corporation. All rights reserved.

import { Neuron, NeuronType, Point3D } from "../..";

/**
 * Configurable options for a {@link Neuron} update.
 */
export type UpdateNeuronOptions = {
  /**
   * The neuron's {@link NeuronType}.
   */
  type?: NeuronType;

  /**
   * The neuron's group identification number.
   */
  group?: number;

  /**
   * The channel the {@link Neuron} maps to.
   */
  channel?: number;

  /**
   * The precise location of the {@link Neuron}.
   */
  position?: Point3D;
};
