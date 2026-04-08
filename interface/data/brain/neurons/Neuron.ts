// Copyright © Spatial Corporation. All rights reserved.

import { NeuronType, Point3D, Schema } from "../..";

/**
 * An autonomous node in a neural network.
 */
export type Neuron = Schema<{
  /**
   * The neuron's {@link NeuronType}.
   */
  type: NeuronType;

  /**
   * The group the {@link Neuron} belongs to.
   */
  group: number;

  /**
   * The channel the {@link Neuron} maps to.
   */
  channel: number;

  /**
   * The neuron's current position.
   */
  position: Point3D;

  /**
   * The neuron's activation level.
   */
  value: number;
}>;
