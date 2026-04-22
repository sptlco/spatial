// Copyright © Spatial Corporation. All rights reserved.

import { Spatial } from "@sptlco/client";
import { Neuron, Snapshot, Synapse } from "@sptlco/data";
import { useEffect, useRef, useState } from "react";

/**
 * Get the current state of the neural network.
 * @returns The current state of the neural network.
 */
export const useBrain = () => {
  const [connecting, setConnecting] = useState(true);

  const [snapshot, setSnapshot] = useState<Snapshot>({ neurons: [], synapses: [] });
  const [neurons, setNeurons] = useState<Record<string, Neuron>>({});
  const [synapses, setSynapses] = useState<Record<string, Synapse>>({});
  const [error, setError] = useState<unknown>(null);

  const abort = useRef<AbortController | null>(null);

  useEffect(() => {
    abort.current = Spatial.brain.stream({
      ready: () => setConnecting(false),
      updated: (ss) => {
        setSnapshot(ss);
      },
      error: (error) => {
        setConnecting(false);
        setError(error);
      },
      neurons: {
        added: (neuron) =>
          setNeurons((v) => ({
            ...v,
            [neuron.id]: neuron
          })),
        updated: (neuron) =>
          setNeurons((v) => ({
            ...v,
            [neuron.id]: neuron
          })),
        removed: (neuron) =>
          setNeurons((v) => {
            const { [neuron.id]: _, ...rest } = v;

            return rest;
          })
      },
      synapses: {
        added: (synapse) =>
          setSynapses((v) => ({
            ...v,
            [synapse.id]: synapse
          })),
        updated: (synapse) =>
          setSynapses((v) => ({
            ...v,
            [synapse.id]: synapse
          })),
        removed: (synapse) =>
          setSynapses((v) => {
            const { [synapse.id]: _, ...rest } = v;

            return rest;
          })
      }
    });

    return () => abort.current?.abort();
  }, []);

  return { connecting, snapshot, neurons, synapses, error };
};
