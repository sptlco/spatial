// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Application } from "@/elements";

import { createElement, Span } from "@sptlco/design";

export const Shipments = createElement<typeof Application.Root>((props, ref) => {
  return (
    <Application.Root title="Shipments">
      <Application.Content>
        <Span className="text-yellow">Hello, world!</Span>
      </Application.Content>
    </Application.Root>
  );
});
