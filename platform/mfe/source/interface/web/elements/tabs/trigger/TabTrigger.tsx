// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-tabs";
import { Element, Node, TabTriggerProps } from "../..";
import clsx from "clsx";

/**
 * Create a new tab trigger element.
 * @param props Configurable options for the element.
 * @returns A tab trigger element.
 */
export const TabTrigger: Element<TabTriggerProps> = (
  props: TabTriggerProps,
): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      value={props.value}
      children={props.children}
      style={props.style}
      className={clsx(
        "overflow-hidden",
        "text-ellipsis whitespace-nowrap",
        "inline-flex shrink-0 grow items-center",
        "disabled:pointer-events-none disabled:opacity-50",
        props.className,
      )}
    />
  );
};
