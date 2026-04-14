// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Application } from "@/elements";
import { useBrain } from "@/hooks";
import { NeuralController, Spatial } from "@sptlco/client";
import { Neuron, Synapse } from "@sptlco/data";
import { useMemo, useRef, useState } from "react";

import { Container, Drawer, Icon, Span } from "@sptlco/design";

import { Explorer } from "./Explorer";
import { Intelligence } from "./Intelligence";

/**
 * A neural interface for the Hypersolver network.
 *
 * Streams live topology and activation state from the server,
 * rendering neurons and synapses in a real-time 3D scene.
 *
 * Supports structural mutations — adding, updating, and removing
 * neurons and synapses via the {@link NeuralController}.
 */
export default function Page() {
  const { connecting, snapshot, neurons, synapses, error } = useBrain();

  const neuronsByGroup = useMemo(() => groupNeurons(neurons), [neurons]);
  const synapsesByGroup = useMemo(() => groupSynapses(synapses, neurons), [synapses, neurons]);

  const [selection, setSelection] = useState<Neuron>();
  const committed = useRef<Neuron | undefined>(undefined);

  const select = (neuron: Neuron | undefined) => {
    if (neuron) {
      committed.current = neuron;
    }

    setSelection(neuron);
  };

  const groups = useMemo(() => {
    return [...new Set([...neuronsByGroup.keys(), ...synapsesByGroup.keys()])].sort((a, b) => a - b);
  }, [neuronsByGroup, synapsesByGroup]);

  if (groups.length === 0) {
    return <p className="text-[10px] text-white/15 tracking-wide px-3 py-2">No neurons in the network.</p>;
  }

  const add = async () => {
    setSelection(
      await Spatial.brain.neurons.add({
        type: "temporal",
        group: 0,
        channel: 0,
        value: 0,
        position: {
          x: 0,
          y: 0,
          z: 0
        }
      })
    );
  };

  return (
    <Application.Root title="Intelligence" className="bg-background-subtle" spacing={false}>
      <Application.Content className="flex min-h-0">
        <Container className="flex flex-col xl:flex-row flex-1 min-h-0 overflow-hidden">
          <Explorer.Root>
            <Explorer.Panel key="entities" title="Explorer">
              <Explorer.Tree type="multiple" onAdd={add}>
                {groups.map((group) => (
                  <Explorer.Group key={group} value={`group-${group}`} name={`Group ${group}`}>
                    {neuronsByGroup.get(group)?.map((n) => (
                      <Explorer.Leaf key={n.id} onSelect={() => select(n)}>
                        <Icon symbol="network_intel_node" size={20} />
                        <Span>Neuron</Span>
                      </Explorer.Leaf>
                    ))}
                    {synapsesByGroup.get(group)?.map((s) => (
                      <Explorer.Leaf key={s.id}>
                        <Span>Synapse</Span>
                      </Explorer.Leaf>
                    ))}
                  </Explorer.Group>
                ))}
              </Explorer.Tree>
            </Explorer.Panel>
          </Explorer.Root>
          <Intelligence snapshot={snapshot} />
        </Container>
      </Application.Content>
      <Drawer.Root open={!!selection} onOpenChange={() => select(undefined)}>
        <Drawer.Content title="Neuron" description={committed.current?.id}></Drawer.Content>
      </Drawer.Root>
    </Application.Root>
  );
}

const groupNeurons = (neurons: Record<string, Neuron>) => {
  const groups = new Map<number, Neuron[]>();

  Object.values(neurons).forEach((n) => {
    if (!groups.has(n.group)) {
      groups.set(n.group, []);
    }

    groups.get(n.group)!.push(n);
  });

  return new Map([...groups.entries()].sort(([a], [b]) => a - b));
};

const groupSynapses = (synapses: Record<string, Synapse>, neurons: Record<string, Neuron>) => {
  const groups = new Map<number, Synapse[]>();

  Object.values(synapses).forEach((s) => {
    const group = neurons[s.from]?.group ?? 0;

    if (!groups.has(group)) {
      groups.set(group, []);
    }

    groups.get(group)!.push(s);
  });

  return groups;
};
