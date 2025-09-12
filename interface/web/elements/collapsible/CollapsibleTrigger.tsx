// Copyright © Spatial Corporation. All rights reserved.

import { DisclosureButton as Primitive } from "@headlessui/react";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new collapsible trigger element.
 * @param props Configurable options for the element.
 * @returns A collapsible trigger element.
 */
export const CollapsibleTrigger: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      as="div"
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};
