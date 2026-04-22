// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Canvas } from "@react-three/fiber";
import { Spatial } from "@sptlco/client";
import { Neuron, Snapshot, Synapse } from "@sptlco/data";
import { Button, Card, Container, createElement, Field, Span } from "@sptlco/design";
import { useState } from "react";

import { Hypersolver } from "./Hypersolver";

export const Intelligence = createElement<
  typeof Card.Root,
  {
    snapshot: Snapshot;
    neurons: Record<string, Neuron>;
    synapses: Record<string, Synapse>;
    onNeuronSelect?: (neuron: Neuron) => void;
    selectedId?: string;
    connectMode?: boolean;
    onConnectModeChange?: (active: boolean) => void;
  }
>(({ snapshot, neurons, synapses, onNeuronSelect, selectedId, connectMode = false, onConnectModeChange, ...props }, ref) => {
  const [connectFrom, setConnectFrom] = useState<string | null>(null);
  const [connectTo, setConnectTo] = useState<string | null>(null);
  const [strength, setStrength] = useState(1);
  const [saving, setSaving] = useState(false);

  const handleSelect = (neuron: Neuron) => {
    if (!connectMode) {
      onNeuronSelect?.(neuron);
      return;
    }

    if (!connectFrom) {
      setConnectFrom(neuron.id);
      return;
    }

    if (neuron.id !== connectFrom) {
      setConnectTo(neuron.id);
    }
  };

  const cancelConnect = () => {
    setConnectFrom(null);
    setConnectTo(null);
    setStrength(1);
  };

  const confirmConnect = async () => {
    if (!connectFrom || !connectTo) return;
    setSaving(true);
    try {
      await Spatial.brain.synapses.add({ from: connectFrom, to: connectTo, strength });
      cancelConnect();
      onConnectModeChange?.(false);
    } finally {
      setSaving(false);
    }
  };

  const phase: ConnectPhase = !connectFrom ? "from" : !connectTo ? "to" : "confirm";

  return (
    <Card.Root {...props} ref={ref} className="flex-1 relative">
      <Card.Content className="w-full flex flex-1 flex-col relative">
        <Canvas className="h-[calc(100vh-(var(--layout-pad)*2)-80px)]!" camera={{ position: [0, 0, 6], fov: 50 }}>
          <Hypersolver
            snapshot={snapshot}
            neurons={neurons}
            synapses={synapses}
            selectedId={connectMode ? (connectFrom ?? undefined) : selectedId}
            onSelect={handleSelect}
          />
        </Canvas>

        {connectMode && (
          <ConnectOverlay
            phase={phase}
            fromNeuron={connectFrom ? neurons[connectFrom] : null}
            toNeuron={connectTo ? neurons[connectTo] : null}
            strength={strength}
            saving={saving}
            onStrengthChange={setStrength}
            onConfirm={confirmConnect}
            onCancel={cancelConnect}
          />
        )}
      </Card.Content>
    </Card.Root>
  );
});

type ConnectPhase = "from" | "to" | "confirm";

type ConnectOverlayProps = {
  phase: ConnectPhase;
  fromNeuron: Neuron | null;
  toNeuron: Neuron | null;
  strength: number;
  saving: boolean;
  onStrengthChange: (v: number) => void;
  onConfirm: () => void;
  onCancel: () => void;
};

const label = (id: string) => id.slice(-6);

const ConnectOverlay = ({ phase, fromNeuron, toNeuron, strength, saving, onStrengthChange, onConfirm, onCancel }: ConnectOverlayProps) => {
  const hint =
    phase === "from"
      ? "Select source neuron"
      : phase === "to"
        ? `From ···${label(fromNeuron!.id)} — select target`
        : `Connect ···${label(fromNeuron!.id)} → ···${label(toNeuron!.id)}`;

  return (
    <Container className="absolute bottom-6 left-1/2 -translate-x-1/2 flex items-center gap-4 px-5 py-3 rounded-2xl bg-background-surface border border-white/10 shadow-xl">
      <Span className="text-xs text-foreground-tertiary whitespace-nowrap font-mono">{hint}</Span>

      {phase === "confirm" && (
        <Field
          type="number"
          id="strength"
          label="Strength"
          inset={false}
          step="0.1"
          value={strength}
          onChange={(e) => {
            const v = parseFloat(e.target.value);
            if (!isNaN(v)) onStrengthChange(v);
          }}
          className="w-24"
        />
      )}

      {phase === "confirm" && (
        <Button intent="highlight" onClick={onConfirm} disabled={saving}>
          Connect
        </Button>
      )}

      {phase !== "from" && (
        <Button intent="ghost" onClick={onCancel}>
          Cancel
        </Button>
      )}
    </Container>
  );
};
