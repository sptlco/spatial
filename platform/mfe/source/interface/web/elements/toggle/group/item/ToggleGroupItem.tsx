// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toggle-group";
import { Element, Node, ToggleGroupItemProps } from "../../..";
import clsx from "clsx";

/**
 * Create a toggle group item element.
 * @param props Configurable options for the element.
 * @returns A toggle group item element.
 */
export const ToggleGroupItem: Element<ToggleGroupItemProps> = (
  props: ToggleGroupItemProps,
): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      style={props.style}
      children={props.children}
      value={props.value}
      disabled={props.disabled}
      className={clsx(
        "transition-all",
        "p-1/2u flex w-fit items-center justify-center",
        "bg-background-interactive-tertiary-default",
        "hover:bg-background-interactive-tertiary-hover",
        "hover:text-foreground-quaternary",
        "data-[state=on]:!bg-background-secondary",
        "data-[state=on]:!text-foreground-primary",
        "data-[disabled]:pointer-events-none",
        "data-[disabled]:cursor-not-allowed",
        "data-[disabled]:opacity-50",
        props.className,
      )}
    />
  );
};
