// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { CollapsibleProps, Element, Node } from "..";
import { Disclosure as Primitive } from "@headlessui/react";

/**
 * Create a new collapsible element.
 * @param props Configurable options for the element.
 * @returns A collapsible element.
 */
export const Collapsible: Element<CollapsibleProps> = (
  props: CollapsibleProps,
): Node => {
  return (
    <Primitive
      as="div"
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
      defaultOpen={props.defaultExpanded}
    />
  );
};
