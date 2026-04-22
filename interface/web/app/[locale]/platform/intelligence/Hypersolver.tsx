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
      <ambientLight intensity={0.08} />
      <pointLight position={[8, 12, 8]} intensity={2.5} color="#00d4ff" />
      <pointLight position={[-12, -8, -6]} intensity={1.2} color="#7700cc" />
      <pointLight position={[0, -10, 14]} intensity={0.8} color="#ffcc44" />

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

      {snapshot.neurons.map((sn) => {
        const neuron = neurons[sn.id];

        if (!neuron) {
          return null;
        }

        return <NeuronMesh key={sn.id} neuron={neuron} activation={sn.value} selected={sn.id === selectedId} onSelect={onSelect} />;
      })}

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

type NeuronMeshProps = {
  neuron: Neuron;
  activation: number;
  selected?: boolean;
  onSelect?: (neuron: Neuron) => void;
};

const NeuronMesh = ({ neuron, activation, onSelect, selected }: NeuronMeshProps) => {
  const coreRef = useRef<THREE.Mesh>(null);
  const shellRef = useRef<THREE.Mesh>(null);

  const color = activationColor(activation);

  useFrame((_, delta) => {
    const t = performance.now() / 1000;

    if (coreRef.current) {
      const pulse = 1.2 + Math.abs(Math.sin(t * 1.8 + neuron.group)) * Math.abs(activation) * 1.6;
      (coreRef.current.material as THREE.MeshStandardMaterial).emissiveIntensity = pulse;
    }

    if (shellRef.current) {
      const breathe = 1 + Math.sin(t * 1.1 + neuron.group * 0.7) * 0.03;
      shellRef.current.scale.setScalar(breathe);
      (shellRef.current.material as THREE.MeshBasicMaterial).opacity = 0.06 + Math.abs(activation) * 0.09;
    }
  });

  return (
    <group position={[neuron.position.x, neuron.position.y, neuron.position.z]} onClick={() => onSelect?.(neuron)}>
      <mesh>
        <sphereGeometry args={[0.3, 8, 8]} />
        <meshBasicMaterial color={color} transparent opacity={0.12} wireframe depthWrite={false} />
      </mesh>

      <mesh ref={shellRef}>
        <sphereGeometry args={[0.3, 16, 16]} />
        <meshBasicMaterial color={color} transparent opacity={0.06} depthWrite={false} />
      </mesh>

      {/* Selection sphere — sits just outside the core, behind the shell */}
      {selected && (
        <mesh>
          <sphereGeometry args={[0.1, 32, 32]} />
          <meshBasicMaterial color="#ffe94d" transparent opacity={0.9} depthWrite={false} />
        </mesh>
      )}

      {/* Arc-reactor core */}
      <mesh ref={coreRef}>
        <sphereGeometry args={[0.1, 32, 32]} />
        <meshStandardMaterial color={color} emissive={color} emissiveIntensity={2} roughness={0.05} metalness={0.95} />
      </mesh>
    </group>
  );
};

type SynapseLineProps = {
  from: { x: number; y: number; z: number };
  to: { x: number; y: number; z: number };
  weight: number;
};

const SynapseLine = ({ from, to, weight }: SynapseLineProps) => {
  const points = useMemo(() => [new THREE.Vector3(from.x, from.y, from.z), new THREE.Vector3(to.x, to.y, to.z)], [from, to]);

  // Positive → arc-reactor cyan, negative → vibranium violet
  const opacity = 0.2 + Math.abs(weight) * 0.7;
  const color = weight >= 0 ? "#00ccff" : "#aa44ff";

  return <Line points={points} color={color} lineWidth={1.2} transparent opacity={Math.min(opacity, 0.85)} />;
};

const activationColor = (value: number): THREE.Color => {
  const clamped = Math.max(-1, Math.min(1, value));

  if (clamped >= 0) {
    return new THREE.Color().lerpColors(new THREE.Color("#1155bb"), new THREE.Color("#c8ffff"), clamped);
  } else {
    return new THREE.Color().lerpColors(new THREE.Color("#6600cc"), new THREE.Color("#1155bb"), clamped + 1);
  }
};

export type HypersolverProps = {
  snapshot: Snapshot;
  neurons: Record<string, Neuron>;
  synapses: Record<string, Synapse>;
  selectedId?: string;
  onSelect?: (neuron: Neuron) => void;
};
