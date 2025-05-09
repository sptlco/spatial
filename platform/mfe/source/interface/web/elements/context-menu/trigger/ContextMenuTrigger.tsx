// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import { ContextMenuTriggerProps, Element, ElementProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new context menu trigger element.
 * @param props Configurable options for the element.
 * @returns A context menu trigger element.
 */
export const ContextMenuTrigger: Element<ContextMenuTriggerProps> = (props: ContextMenuTriggerProps): Node => {
  return (
    <Primitive.Trigger
      ref={props.ref}
      style={props.style}
      className={clsx("h-fit w-fit", props.className)}
      asChild={props.asChild}
      children={props.children}
    />
  );
};
