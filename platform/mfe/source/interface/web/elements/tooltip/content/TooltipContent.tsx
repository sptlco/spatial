// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-tooltip";
import { Element, Node, TooltipContentProps } from "../..";
import clsx from "clsx";

/**
 * Create a new tooltip content element.
 * @param props Configurable options for the element.
 * @returns A tooltip content element.
 */
export const TooltipContent: Element<TooltipContentProps> = (
  props: TooltipContentProps,
): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Content
        ref={props.ref}
        asChild={props.asChild}
        side={props.side}
        align={props.align}
        sideOffset={8}
        collisionPadding={8}
        style={props.style}
        children={props.children}
        className={clsx(
          "py-3/4u px-1u",
          "rounded-1/2u",
          "bg-background-accent text-base-white-9",
          "max-w-[var(--radix-popover-content-available-width)]",
          "max-h-[var(--radix-popover-content-available-height)]",
          "data-[side=top]:min-w-[var(--radix-popover-trigger-width)]",
          "data-[side=bottom]:min-w-[var(--radix-popover-trigger-width)]",
          "data-[side=left]:min-h-[var(--radix-popover-trigger-height)]",
          "data-[side=right]:min-h-[var(--radix-popover-trigger-height)]",
          "data-[state=delayed-open]:animate-grow data-[state=closed]:animate-shrink flex origin-top transition-all",
          props.className,
        )}
      />
    </Primitive.Portal>
  );
};
