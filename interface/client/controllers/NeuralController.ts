// Copyright © Spatial Corporation. All rights reserved.

import { fetchEventSource } from "@microsoft/fetch-event-source";
import cookies from "js-cookie";

import {
  CreateNeuronOptions,
  CreateSynapseOptions,
  Neuron,
  Snapshot,
  StimulateNeuronOptions,
  StreamOptions,
  Synapse,
  UpdateNeuronOptions,
  UpdateSynapseOptions
} from "@sptlco/data";

import { Controller, SESSION_COOKIE_NAME } from "..";

/**
 * A {@link Controller} for neural functions.
 */
export class NeuralController extends Controller {
  /**
   * Get a {@link Snapshot} of the neural network.
   * @returns A {@link Snapshot} of the neural network.
   */
  public snapshot = () => this.get<Snapshot>("brain");

  /**
   * Stream the neural network.
   * @param options Configurable options for the stream.
   * @returns An {@link AbortController}.
   */
  public stream = (options?: StreamOptions) => {
    const controller = new AbortController();
    const session = cookies.get(SESSION_COOKIE_NAME);

    fetchEventSource(`${process.env.NEXT_PUBLIC_SERVER_ENDPOINT}/brain/stream`, {
      headers: session ? { Authorization: `Bearer ${session}` } : {},
      signal: controller.signal,
      onmessage: (message) => {
        try {
          const data = JSON.parse(message.data);

          switch (message.event) {
            case "ready":
              options?.ready?.();
              break;
            case "updated":
              options?.updated?.(data);
              break;
            case "neurons.add":
              options?.neurons?.added?.(data);
              break;
            case "neurons.update":
              options?.neurons?.updated?.(data);
              break;
            case "neurons.remove":
              options?.neurons?.removed?.(data);
              break;
            case "synapses.add":
              options?.synapses?.added?.(data);
              break;
            case "synapses.update":
              options?.synapses?.updated?.(data);
              break;
            case "synapses.remove":
              options?.synapses?.removed?.(data);
              break;
          }
        } catch {
          // Malformed frame. Skip.
        }
      },
      onerror: (error) => {
        if (controller.signal.aborted) {
          return;
        }

        options?.error?.(error);

        throw error;
      }
    });

    return controller;
  };

  /**
   * Endpoints for neural mutations.
   */
  public neurons = {
    /**
     * Stimulate a {@link Neuron}.
     * @param neuron The {@link Neuron} to stimulate.
     * @param options Configurable options for {@link Neuron} stimulation.
     */
    stimulate: (neuron: string, options: StimulateNeuronOptions) => this.post(`brain/neurons/${neuron}/stimulate`, options),

    /**
     * Add a {@link Neuron} to the neural network.
     * @param options Configurable options for the {@link Neuron}.
     * @returns The {@link Neuron} that was added.
     */
    add: (options: CreateNeuronOptions) => this.post<Neuron>("brain/neurons", options),

    /**
     * Update a {@link Neuron}.
     * @param neuron The {@link Neuron} to update.
     * @param options Configurable options for the {@link Neuron} update.
     */
    update: (neuron: string, options: UpdateNeuronOptions) => this.patch(`brain/neurons/${neuron}`, options),

    /**
     * Remove a {@link Neuron} from the neural network.
     * @param neuron The {@link Neuron} to remove.
     */
    remove: (neuron: string) => this.delete(`brain/neurons/${neuron}`)
  };

  /**
   * Endpoints for synaptic mutations.
   */
  public synapses = {
    /**
     * Add a {@link Synapse} to the neural network.
     * @param options Configurable options for the {@link Synapse}.
     * @returns The {@link Synapse} that was added.
     */
    add: (options: CreateSynapseOptions) => this.post<Synapse>("brain/synapses", options),

    /**
     * Update a {@link Synapse}.
     * @param synapse The {@link Synapse} to update.
     * @param options Configurable options for the {@link Synapse} update.
     */
    update: (synapse: string, options: UpdateSynapseOptions) => this.patch(`brain/synapses/${synapse}`, options),

    /**
     * Remove a {@link Synapse} from the neural network.
     * @param synapse The {@link Synapse} to remove.
     */
    remove: (synapse: string) => this.delete(`brain/synapses/${synapse}`)
  };
}
