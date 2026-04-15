// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Canvas } from "@react-three/fiber";
import { Neuron, Snapshot, Synapse } from "@sptlco/data";
import { Card, createElement } from "@sptlco/design";

import { Hypersolver } from "./Hypersolver";

export const Intelligence = createElement<
  typeof Card.Root,
  {
    snapshot: Snapshot;
    neurons: Record<string, Neuron>;
    synapses: Record<string, Synapse>;
    onNeuronSelect?: (neuron: Neuron) => void;
    selectedId?: string;
  }
>(({ snapshot, neurons, synapses, onNeuronSelect, selectedId, ...props }, ref) => {
  return (
    <Card.Root {...props} ref={ref} className="flex-1 relative">
      <Card.Content className="w-full flex flex-1 flex-col relative">
        <Canvas className="h-[calc(100vh-(var(--layout-pad)*2)-80px)]!" camera={{ position: [0, 0, 6], fov: 50 }}>
          <Hypersolver snapshot={snapshot} neurons={neurons} synapses={synapses} selectedId={selectedId} onSelect={onNeuronSelect} />
        </Canvas>
      </Card.Content>
    </Card.Root>
  );
});
