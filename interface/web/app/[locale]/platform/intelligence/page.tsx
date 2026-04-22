// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Application } from "@/elements";
import { useBrain } from "@/hooks";
import { NeuralController, Spatial } from "@sptlco/client";
import { Neuron, Synapse } from "@sptlco/data";
import { useMemo, useRef, useState } from "react";

import { Button, Container, Drawer, Empty, Icon, Span } from "@sptlco/design";

import { Editor } from "./Editor";
import { Explorer } from "./Explorer";
import { Intelligence } from "./Intelligence";

/**
 * A neural interface for the Hypersolver network.
 */
export default function Page() {
  const { connecting, snapshot, neurons, synapses, error } = useBrain();

  const neuronsByGroup = useMemo(() => groupNeurons(neurons), [neurons]);
  const synapsesByGroup = useMemo(() => groupSynapses(synapses, neurons), [synapses, neurons]);

  const [selection, setSelection] = useState<Neuron>();
  const [connectMode, setConnectMode] = useState(false);
  const committed = useRef<Neuron | undefined>(undefined);

  const select = (neuron?: Neuron) => {
    if (neuron) committed.current = neuron;
    setSelection(neuron);
  };

  const enterConnectMode = () => {
    setSelection(undefined); // close drawer so it doesn't obscure the canvas
    setConnectMode(true);
  };

  const groups = useMemo(
    () => [...new Set([...neuronsByGroup.keys(), ...synapsesByGroup.keys()])].sort((a, b) => a - b),
    [neuronsByGroup, synapsesByGroup]
  );

  const add = async () => {
    select(
      await Spatial.brain.neurons.add({
        type: "temporal",
        group: 0,
        channel: 0,
        value: 0,
        position: { x: 0, y: 0, z: 0 }
      })
    );
  };

  const renderGroups = () => {
    if (groups.length === 0) {
      return (
        <Empty.Root className="h-full">
          <Empty.Header>
            <Empty.Media variant="icon">
              <Icon symbol="network_intelligence" />
            </Empty.Media>
            <Empty.Title>The network is empty</Empty.Title>
            <Empty.Description>There are currently no nodes in the network. To get started, add a neuron.</Empty.Description>
          </Empty.Header>
          <Empty.Content>
            <Button onClick={add}>Add Neuron</Button>
          </Empty.Content>
        </Empty.Root>
      );
    }

    return (
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
    );
  };

  const activeNeuron = selection ?? committed.current;
  const activation = activeNeuron?.value ?? 0;

  return (
    <Application.Root title="Intelligence" className="bg-background-subtle" spacing={false}>
      <Application.Content className="flex min-h-0">
        <Container className="flex flex-col xl:flex-row flex-1 min-h-0 overflow-hidden">
          <Explorer.Root>
            <Explorer.Panel key="entities" title="Explorer">
              {renderGroups()}
            </Explorer.Panel>
          </Explorer.Root>

          <Container className="relative flex flex-1 min-h-0">
            <Intelligence
              snapshot={snapshot}
              neurons={neurons}
              synapses={synapses}
              selectedId={selection?.id}
              onNeuronSelect={select}
              connectMode={connectMode}
              onConnectModeChange={setConnectMode}
            />

            <Container className="absolute top-4 right-4 flex gap-2">
              <Button
                intent={connectMode ? "highlight" : "ghost"}
                size="fit"
                className="px-3 py-2 rounded-xl gap-2"
                onClick={connectMode ? () => setConnectMode(false) : enterConnectMode}
              >
                <Icon symbol="cable" size={18} />
                <Span className="text-xs">{connectMode ? "Cancel" : "Connect"}</Span>
              </Button>
            </Container>
          </Container>
        </Container>
      </Application.Content>

      <Drawer.Root open={!!selection} onOpenChange={() => select(undefined)} direction="right">
        <Drawer.Content title="Neuron" description={committed.current?.id} overlay={false} closeButton>
          {activeNeuron && (
            <Editor
              neuron={activeNeuron}
              activation={activation}
              onCommit={(updated) => {
                committed.current = updated;
                setSelection(updated);
              }}
            />
          )}
        </Drawer.Content>
      </Drawer.Root>
    </Application.Root>
  );
}

const groupNeurons = (neurons: Record<string, Neuron>) => {
  const groups = new Map<number, Neuron[]>();
  Object.values(neurons).forEach((n) => {
    if (!groups.has(n.group)) groups.set(n.group, []);
    groups.get(n.group)!.push(n);
  });
  return new Map([...groups.entries()].sort(([a], [b]) => a - b));
};

const groupSynapses = (synapses: Record<string, Synapse>, neurons: Record<string, Neuron>) => {
  const groups = new Map<number, Synapse[]>();
  Object.values(synapses).forEach((s) => {
    const group = neurons[s.from]?.group ?? 0;
    if (!groups.has(group)) groups.set(group, []);
    groups.get(group)!.push(s);
  });
  return groups;
};
