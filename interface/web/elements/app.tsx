// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";

import { Header } from "./header";

import { createElement, Card } from "@sptlco/design";

export const Application = {
  Spacing: "xl:pr-10 py-10 xl:pt-0 gap-10 xl:gap-20",

  Root: createElement<typeof Card.Root, { title?: string; spacing?: boolean }>(({ title, spacing = true, ...props }, ref) => (
    <Card.Root {...props} ref={ref} className={clsx("flex-1", spacing && Application.Spacing, props.className)}>
      <Header title={title} />
      {props.children}
    </Card.Root>
  )),

  Content: createElement<typeof Card.Content>((props, ref) => (
    <Card.Content {...props} ref={ref} className={clsx("flex flex-1 min-h-0 flex-col w-full gap-10 xl:gap-20", props.className)} />
  ))
};
