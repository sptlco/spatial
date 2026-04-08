// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Application } from "@/elements";
import { useBrain } from "@/hooks";
import { NeuralController } from "@sptlco/client";
import { clsx } from "clsx";

import { Container } from "@sptlco/design";

import { Explorer } from "./explorer/Explorer";
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

  return (
    <Application.Root title="Intelligence" className="bg-background-subtle" spacing={false}>
      <Application.Content className="flex min-h-0">
        <Container className="flex flex-1 min-h-0 overflow-hidden">
          <Explorer entities={[]} />
          <Intelligence className={clsx("flex-1 min-h-0 xl:pt-10! xl:pb-0!", Application.Spacing)} />
        </Container>
      </Application.Content>
    </Application.Root>
  );
}
