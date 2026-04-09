// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Canvas } from "@react-three/fiber";
import { NeuralController } from "@sptlco/client";
import { Snapshot } from "@sptlco/data";
import { Card, createElement, Span } from "@sptlco/design";

import { Hypersolver } from "./Hypersolver";

/**
 * The neural interface for the Hypersolver network.
 *
 * Streams live topology and activation state from the server,
 * rendering neurons and synapses in a real-time 3D scene.
 *
 * Supports structural mutations — adding, updating, and removing
 * neurons and synapses via the {@link NeuralController}.
 */
export const Intelligence = createElement<typeof Card.Root, { snapshot: Snapshot }>(({ snapshot, ...props }, ref) => {
  return (
    <Card.Root {...props} ref={ref} className="flex-1 relative">
      <Card.Header>
        <Card.Title className="absolute text-2xl font-bold flex gap-3 items-center top-10 xl:top-6 px-10">
          <Span>Intelligence</Span>
        </Card.Title>
      </Card.Header>
      <Card.Content className="w-full flex flex-1 flex-col relative">
        <Canvas>
          <Hypersolver snapshot={snapshot} />
        </Canvas>
      </Card.Content>
    </Card.Root>
  );
});
