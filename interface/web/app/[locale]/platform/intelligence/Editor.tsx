// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Canvas, useFrame } from "@react-three/fiber";
import { Spatial } from "@sptlco/client";
import { Neuron, NEURON_TYPES } from "@sptlco/data";
import { Button, Container, Drawer, Field, Form, Span } from "@sptlco/design";
import { useMemo, useRef, useState } from "react";
import * as THREE from "three";

import { Stimulate } from "./Stimulate";

type NeuronEditorProps = {
  neuron: Neuron;
  activation?: number;
  onCommit?: (updated: Neuron) => void;
};

export const Editor = ({ neuron, activation = 0, onCommit }: NeuronEditorProps) => {
  const [draft, setDraft] = useState<Neuron>({ ...neuron });
  const [saving, setSaving] = useState(false);

  const set = <K extends keyof Neuron>(key: K, value: Neuron[K]) => setDraft((prev) => ({ ...prev, [key]: value }));

  const setPosition = (axis: "x" | "y" | "z", raw: string) => {
    const n = parseFloat(raw);
    if (!isNaN(n)) setDraft((prev) => ({ ...prev, position: { ...prev.position, [axis]: n } }));
  };

  const commit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    try {
      await Spatial.brain.neurons.update(neuron.id, draft);
      onCommit?.(draft);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="flex flex-col gap-5">
      <div className="h-36 overflow-hidden">
        <Canvas camera={{ position: [0, 0, 1.4], fov: 45 }}>
          <NeuronPreview activation={activation} />
        </Canvas>
      </div>

      <Stimulate neuron={neuron} />

      <hr className="border-white/8" />

      <Form className="flex flex-col gap-10" onSubmit={commit}>
        <Field
          type="option"
          id="type"
          label="Type"
          inset={false}
          options={NEURON_TYPES.map((t) => ({ value: t, label: t.charAt(0).toUpperCase() + t.slice(1) }))}
          selection={draft.type}
          onValueChange={(v) => set("type", v as Neuron["type"])}
        />

        <Container className="grid grid-cols-2 gap-3">
          <Field
            type="number"
            id="group"
            label="Group"
            inset={false}
            value={draft.group}
            onChange={(e) => set("group", parseInt(e.target.value) || 0)}
          />
          <Field
            type="number"
            id="channel"
            label="Channel"
            inset={false}
            value={draft.channel}
            onChange={(e) => set("channel", parseInt(e.target.value) || 0)}
          />
        </Container>

        <Container className="flex flex-col gap-4">
          <Container className="flex items-center">
            <Span className="font-extrabold text-xs uppercase text-hint px-0 grow">Position</Span>
          </Container>
          <Container className="grid grid-cols-3 gap-2">
            {(["x", "y", "z"] as const).map((axis) => (
              <Field
                key={axis}
                type="number"
                id={`position-${axis}`}
                label={axis.toUpperCase()}
                inset={false}
                step="0.1"
                value={draft.position[axis]}
                onChange={(e) => setPosition(axis, e.target.value)}
              />
            ))}
          </Container>
        </Container>

        <Container className="flex items-center gap-4">
          <Button type="submit" intent="highlight" disabled={saving}>
            Commit
          </Button>
          <Drawer.Close asChild>
            <Button>Cancel</Button>
          </Drawer.Close>
        </Container>
      </Form>
    </div>
  );
};

const NeuronPreview = ({ activation }: { activation: number }) => {
  const meshRef = useRef<THREE.Mesh>(null);
  const ringRef = useRef<THREE.Mesh>(null);
  const color = useMemo(() => previewColor(activation), [activation]);

  useFrame(({ clock }) => {
    const t = clock.getElapsedTime();

    if (meshRef.current) {
      meshRef.current.rotation.y = t * 0.6;
      meshRef.current.rotation.x = Math.sin(t * 0.3) * 0.2;
      (meshRef.current.material as THREE.MeshStandardMaterial).emissiveIntensity = 0.4 + Math.sin(t * 2) * 0.2 * Math.abs(activation);
    }

    if (ringRef.current) {
      ringRef.current.rotation.z = t * 0.4;
      ringRef.current.rotation.x = Math.PI / 2 + Math.sin(t * 0.5) * 0.3;
    }
  });

  return (
    <>
      <ambientLight intensity={0.5} />
      <pointLight position={[2, 2, 2]} intensity={1.5} />
      <pointLight position={[-2, -1, -2]} intensity={0.4} color="#6080ff" />
      <mesh ref={meshRef}>
        <sphereGeometry args={[0.38, 48, 48]} />
        <meshStandardMaterial color={color} emissive={color} emissiveIntensity={0.5} roughness={0.15} metalness={0.5} />
      </mesh>
      <mesh ref={ringRef}>
        <torusGeometry args={[0.55, 0.018, 16, 80]} />
        <meshStandardMaterial color={color} emissive={color} emissiveIntensity={0.8} roughness={0.1} metalness={0.6} transparent opacity={0.6} />
      </mesh>
    </>
  );
};

const previewColor = (activation: number): THREE.Color => {
  const clamped = Math.max(-1, Math.min(1, activation));
  return clamped >= 0
    ? new THREE.Color().lerpColors(new THREE.Color("#8899ff"), new THREE.Color("#ff6030"), clamped)
    : new THREE.Color().lerpColors(new THREE.Color("#2040c0"), new THREE.Color("#8899ff"), clamped + 1);
};
