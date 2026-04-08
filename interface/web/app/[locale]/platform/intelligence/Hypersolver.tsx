// Copyright © Spatial Corporation. All rights reserved.

import { useFrame } from "@react-three/fiber";
import { Snapshot } from "@sptlco/data";
import { createElement } from "@sptlco/design";
import { Fragment } from "react";

/**
 * A real-time 3D view of the Hypersolver network.
 *
 * The Hypersolver is a custom neural network leveraging temporal dynamics
 * for continuous state changes over time.
 */
export const Hypersolver = createElement<typeof Fragment, HypersolverProps>(({ snapshot, ...props }, _) => {
  useFrame(({ scene }) => {
    // ...
  });

  return <Fragment {...props} />;
});

/**
 * Configurable options for the {@link Hypersolver}.
 */
export type HypersolverProps = {
  /**
   * The current state of the {@link Hypersolver} network.
   */
  snapshot: Snapshot;
};
