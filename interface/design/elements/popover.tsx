// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-popover";
import { clsx } from "clsx";
import { cva } from "cva";

import { createElement, ScrollArea } from "..";

/**
 * Displays rich content in a portal, triggered by a button.
 */
export const Popover = {
  ...Primitive,

  Content: createElement<typeof Primitive.Content>((props, ref) => {
    const content = cva({
      base: [
        "w-fit rounded-xl duration-100 z-50 overflow-x-hidden",
        "bg-background-surface shadow-base relative",
        "data-[state=open]:animate-in data-[state=open]:fade-in-0 data-[state=open]:zoom-in-95",
        "data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95",
        "max-h-(--radix-popover-content-available-height) origin-(--radix-popover-content-transform-origin)"
      ],
      variants: {
        side: {
          left: "data-[state=open]:slide-in-from-right-2 data-[state=closed]:slide-out-to-right-2",
          right: "data-[state=open]:slide-in-from-left-2 data-[state=closed]:slide-out-to-left-2",
          top: "data-[state=open]:slide-in-from-bottom-2 data-[state=closed]:slide-out-to-bottom-2",
          bottom: "data-[state=open]:slide-in-from-top-2 data-[state=closed]:slide-out-to-top-2"
        }
      }
    });

    return (
      <Primitive.Portal>
        <Primitive.Content
          {...props}
          ref={ref}
          sideOffset={20}
          collisionPadding={40}
          avoidCollisions
          className={clsx(content({ side: props.side }), props.className)}
          style={{ overflowY: "hidden" }}
        />
      </Primitive.Portal>
    );
  }),

  /**
   * Contains the scrollable parts of a popover.
   */
  Viewport: createElement<typeof ScrollArea.Viewport>((props, ref) => (
    <ScrollArea.Root className="grow" type="auto" fade>
      <ScrollArea.Viewport {...props} ref={ref} />
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  ))
};
