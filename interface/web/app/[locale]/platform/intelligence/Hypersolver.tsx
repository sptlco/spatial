// Copyright © Spatial Corporation. All rights reserved.

import { Line, OrbitControls, Grid } from "@react-three/drei";
import { useFrame } from "@react-three/fiber";
import { Neuron, Snapshot, Synapse } from "@sptlco/data";
import { createElement } from "@sptlco/design";
import { Fragment, useMemo, useRef } from "react";
import * as THREE from "three";

/**
 * A real-time 3D view of the Hypersolver network.
 *
 * Neurons are rendered as emissive spheres whose color reflects their
 * current activation value. Synapses are drawn as lines connecting
 * source and target neurons.
 */
export const Hypersolver = createElement<typeof Fragment, HypersolverProps>(({ snapshot, neurons, synapses, onSelect, selectedId, ...props }, _) => {
  useFrame(({ camera }) => {
    camera.near = 0.01;
    camera.updateProjectionMatrix();
  });

  return (
    <Fragment {...props}>
      <ambientLight intensity={0.4} />
      <pointLight position={[10, 10, 10]} intensity={1.2} />
      <pointLight position={[-10, -10, -10]} intensity={0.4} color="#6080ff" />
      <Grid
        renderOrder={-1}
        position={[0, -2, 0]}
        args={[50, 50]}
        cellSize={1}
        cellThickness={0.5}
        cellColor="#666666"
        sectionSize={5}
        sectionThickness={2}
        sectionColor="#333333"
        fadeDistance={60}
        fadeStrength={1}
        infiniteGrid
      />
      <OrbitControls makeDefault enableDamping dampingFactor={0.08} />

      {Object.values(neurons).map((neuron) => (
        <NeuronMesh key={neuron.id} neuron={neuron} activation={neuron.value} selected={neuron.id === selectedId} onSelect={onSelect} />
      ))}

      {snapshot.synapses.map((synapse) => {
        const from = neurons[synapse.from];
        const to = neurons[synapse.to];

        if (!from || !to) {
          return null;
        }

        return <SynapseLine key={synapse.id} from={from.position} to={to.position} weight={synapse.strength ?? 0} />;
      })}
    </Fragment>
  );
});

// ---------------------------------------------------------------------------
// NeuronMesh
// ---------------------------------------------------------------------------

type NeuronMeshProps = {
  neuron: Neuron;
  activation: number;
  selected?: boolean; // 👈 new
  onSelect?: (neuron: Neuron) => void;
};

const NeuronMesh = ({ neuron, activation, onSelect, selected }: NeuronMeshProps) => {
  const meshRef = useRef<THREE.Mesh>(null);
  const color = activationColor(activation);

  useFrame((_, delta) => {
    if (!meshRef.current) {
      return;
    }

    const t = performance.now() / 1000;
    const pulse = 0.3 + Math.abs(Math.sin(t * 1.4 + neuron.group)) * Math.abs(activation) * 0.6;

    (meshRef.current.material as THREE.MeshStandardMaterial).emissiveIntensity = pulse;

    if (selected && meshRef.current) {
      const outline = meshRef.current.parent?.children[0] as THREE.Mesh;

      if (outline) {
        const t = performance.now() / 1000;
        outline.scale.setScalar(1 + Math.sin(t * 4) * 0.05);
      }
    }
  });

  return (
    <group position={[neuron.position.x, neuron.position.y, neuron.position.z]} onClick={() => onSelect?.(neuron)}>
      {selected && (
        <mesh>
          <sphereGeometry args={[0.24, 32, 32]} />
          <meshBasicMaterial color="#ffe94d" transparent opacity={0.9} depthWrite={false} />
        </mesh>
      )}
      <mesh ref={meshRef}>
        <sphereGeometry args={[0.18, 32, 32]} />
        <meshStandardMaterial color={color} emissive={color} emissiveIntensity={0.8} roughness={0.15} metalness={0.6} />
      </mesh>
    </group>
  );
};

// ---------------------------------------------------------------------------
// SynapseLine
// ---------------------------------------------------------------------------

type SynapseLineProps = {
  from: { x: number; y: number; z: number };
  to: { x: number; y: number; z: number };
  weight: number;
};

const SynapseLine = ({ from, to, weight }: SynapseLineProps) => {
  const points = useMemo(() => [new THREE.Vector3(from.x, from.y, from.z), new THREE.Vector3(to.x, to.y, to.z)], [from, to]);

  const opacity = 0.15 + Math.abs(weight) * 0.6;
  const color = weight >= 0 ? "#6090ff" : "#ff6060";

  return <Line points={points} color={color} lineWidth={1} transparent opacity={Math.min(opacity, 0.8)} />;
};

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

/** Maps an activation value in [-1, 1] to a Three.js color. */
const activationColor = (value: number): THREE.Color => {
  const clamped = Math.max(-1, Math.min(1, value));

  if (clamped >= 0) {
    // Neutral → hot orange-red
    return new THREE.Color().lerpColors(new THREE.Color("#8899ff"), new THREE.Color("#ff6030"), clamped);
  } else {
    // Cool blue → neutral
    return new THREE.Color().lerpColors(new THREE.Color("#2040c0"), new THREE.Color("#8899ff"), clamped + 1);
  }
};

// ---------------------------------------------------------------------------
// Types
// ---------------------------------------------------------------------------

export type HypersolverProps = {
  snapshot: Snapshot;
  neurons: Record<string, Neuron>;
  synapses: Record<string, Synapse>;
  selectedId?: string; // 👈 add
  onSelect?: (neuron: Neuron) => void;
};
