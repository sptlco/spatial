// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-popover";
import { clsx } from "clsx";

/**
 * Displays rich content in a portal, triggered by a button.
 */
export const Popover = {
  ...Primitive,

  /**
   * Contains all the parts of a dropdown menu.
   */
  Root: createElement<typeof Primitive.Root>((props, _) => <Primitive.Root {...props} />),

  /**
   * The button that toggles the dropdown menu.
   */
  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} className={clsx("cursor-pointer", props.className)} />
  )),

  /**
   * When used, portals the content part into the body.
   */
  Portal: createElement<typeof Primitive.Portal>((props, _) => <Primitive.Portal {...props} />),

  /**
   * The component that pops out when the dropdown menu is open.
   */
  Content: createElement<typeof Primitive.Content>((props, ref) => (
    <Primitive.Portal>
      <Primitive.Content
        {...props}
        ref={ref}
        sideOffset={20}
        collisionPadding={40}
        avoidCollisions
        className={clsx(
          "z-50",
          "flex flex-col gap-4",
          "bg-background-surface text-sm shadow-lg rounded-xl p-3 w-fit md:w-screen md:max-w-52",
          "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2",
          props.className
        )}
      />
    </Primitive.Portal>
  )),

  /**
   * An optional arrow element to render alongside the dropdown menu.
   */
  Arrow: createElement<typeof Primitive.Arrow>((props, ref) => <Primitive.Arrow {...props} ref={ref} />)
};
