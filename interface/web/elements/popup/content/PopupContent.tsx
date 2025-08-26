// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-popover";
import { Element, Node, PopupContentProps } from "../..";
import clsx from "clsx";

/**
 * Create a new popup content element.
 * @param props Configurable options for the element.
 * @returns A popup content element.
 */
export const PopupContent: Element<PopupContentProps> = (
  props: PopupContentProps,
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
          "z-50",
          "py-3/4u px-1u",
          "rounded-1/2u",
          "bg-background-primary",
          "max-w-[var(--radix-popover-content-available-width)]",
          "data-[side=top]:min-w-[var(--radix-popover-trigger-width)]",
          "data-[side=bottom]:min-w-[var(--radix-popover-trigger-width)]",
          "data-[side=left]:min-h-[var(--radix-popover-trigger-height)]",
          "data-[side=right]:min-h-[var(--radix-popover-trigger-height)]",
          "data-[state=delayed-open]:animate-grow data-[state=closed]:animate-shrink flex origin-top transition-all",
          "outline-border-bounds outline outline-1",
          props.className,
        )}
      />
    </Primitive.Portal>
  );
};
