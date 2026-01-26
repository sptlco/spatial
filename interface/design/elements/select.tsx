// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon } from "..";
import * as Primitive from "@radix-ui/react-select";
import { clsx } from "clsx";
import { cva } from "cva";

/**
 * Displays a list of options for the user to pick from.
 */
export const Select = {
  ...Primitive,

  /**
   * The button that toggles the select.
   */
  Trigger: createElement<typeof Primitive.Trigger, { placeholder?: string }>(({ children, ...props }, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "cursor-pointer flex items-center gap-2",
        "rounded-lg px-8 py-2 transition-all",
        "bg-button-ghost hover:bg-button-ghost-hover active:bg-button-ghost-active data-[state=open]:bg-button-ghost-active",
        props.className
      )}
    >
      <Primitive.Value placeholder={props.placeholder}>{children}</Primitive.Value>
      <Primitive.Icon asChild>
        <Icon symbol="keyboard_arrow_down" />
      </Primitive.Icon>
    </Primitive.Trigger>
  )),

  /**
   * The component that pops out when the select is open.
   */
  Content: createElement<typeof Primitive.Content>(({ side = "bottom", ...props }, ref) => {
    const classes = cva({
      base: [
        "bg-background-surface text-sm shadow-lg rounded-xl p-3 w-screen max-w-48 sm:max-w-3xs",
        "data-[state=open]:animate-in data-[state=open]:fade-in data-[state=open]:zoom-in-95",
        "data-[state=closed]:animate-out data-[state=closed]:fade-out data-[state=closed]:zoom-out-95"
      ],
      variants: {
        side: {
          top: [
            "data-[state=open]:animate-in data-[state=open]:slide-in-from-bottom-10",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-bottom-10"
          ],
          bottom: [
            "data-[state=open]:animate-in data-[state=open]:slide-in-from-top-10",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-top-10"
          ],
          left: [
            "data-[state=open]:animate-in data-[state=open]:slide-in-from-right-10",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-right-10"
          ],
          right: [
            "data-[state=open]:animate-in data-[state=open]:slide-in-from-left-10",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-left-10"
          ]
        }
      }
    });

    return (
      <Primitive.Portal>
        <Primitive.Content {...props} ref={ref} sideOffset={20} collisionPadding={40} className={clsx(classes({ side }), props.className)} />
      </Primitive.Portal>
    );
  })
};
