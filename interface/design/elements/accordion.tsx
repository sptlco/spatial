// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-accordion";
import { clsx } from "clsx";

import { createElement } from "..";

/**
 * A vertically stacked set of interactive headings that each reveal an associated section of content.
 */
export const Accordion = {
  ...Primitive,

  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger {...props} className={clsx("cursor-pointer", props.className)} ref={ref} />
  )),

  Content: createElement<typeof Primitive.Content, { animate?: boolean }>(({ animate, ...props }, ref) => (
    <Primitive.Content
      {...props}
      ref={ref}
      className={clsx(
        "overflow-hidden",
        animate && "data-[state=open]:animate-accordion-down data-[state=closed]:animate-accordion-up",
        props.className
      )}
    />
  ))
};
