// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Neuron } from "@sptlco/data";
import { Button, Container, Slider, Span } from "@sptlco/design";
import { useState } from "react";

type StimulateProps = {
  neuron: Neuron;
};

/**
 * Delivers a one-shot charge to a neuron via the stimulate endpoint.
 * Embedded in the Editor drawer below the commit form.
 */
export const Stimulate = ({ neuron }: StimulateProps) => {
  const [charge, setCharge] = useState(0.5);
  const [loading, setLoading] = useState(false);

  const fire = async () => {
    setLoading(true);

    try {
      await Spatial.brain.neurons.stimulate(neuron.id, { charge });
    } finally {
      setLoading(false);
    }
  };

  const sign = charge >= 0 ? "+" : "";

  return (
    <Container className="flex flex-col gap-4">
      <Container className="flex items-center justify-between">
        <Span className="font-extrabold text-xs uppercase text-foreground-quaternary">Stimulate</Span>
        <Span className="text-xs font-mono text-foreground-tertiary">
          {sign}
          {charge.toFixed(2)}
        </Span>
      </Container>

      <Slider min={-1} max={1} step={0.01} value={[charge]} onValueChange={([v]) => setCharge(v)} />

      <Button intent="highlight" onClick={fire} disabled={loading}>
        {loading ? "Firing…" : "Fire"}
      </Button>
    </Container>
  );
};
