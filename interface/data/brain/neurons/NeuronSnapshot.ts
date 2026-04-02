// Copyright © Spatial Corporation. All rights reserved.

import { Neuron, NeuronType, Point3D } from "../..";

/**
 * An immutable point-in-time view of a {@link Neuron}.
 */
export type NeuronSnapshot = {
  /**
   * The neuron's unique identifier.
   */
  id: string;

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
