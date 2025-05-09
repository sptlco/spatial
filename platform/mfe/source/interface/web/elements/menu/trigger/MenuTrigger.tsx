// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, MenuTriggerProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new menu trigger element.
 * @param props Configurable options for the element.
 * @returns A menu trigger element.
 */
export const MenuTrigger: Element<MenuTriggerProps> = (
  props: MenuTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      style={props.style}
      children={props.children}
      disabled={props.disabled}
      className={clsx(
        "transition-all",
        "rounded-1/2u !outline-none",
        "px-1u py-1/4u space-x-1/2u inline-flex items-center",
        "data-[disabled]:pointer-events-none data-[disabled]:opacity-50",
        "data-[highlighted]:bg-background-tertiary hover:bg-background-secondary",
        "data-[state=open]:!bg-background-accent",
        "data-[state=open]:!text-base-white-9",
        props.className,
      )}
    />
  );
};
