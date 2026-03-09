// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";

import { Header } from "./header";

import { createElement, Card } from "@sptlco/design";

export const Application = {
  Root: createElement<typeof Card.Root, { title?: string }>(({ title, ...props }, ref) => (
    <Card.Root {...props} ref={ref}>
      <Header title={title} />
      {props.children}
    </Card.Root>
  )),

  Content: createElement<typeof Card.Content>((props, ref) => (
    <Card.Content {...props} ref={ref} className={clsx("flex flex-col gap-10 xl:gap-20", props.className)} />
  ))
};
