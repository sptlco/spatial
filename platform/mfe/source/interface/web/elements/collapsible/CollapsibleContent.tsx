// Copyright © Spatial. All rights reserved.

import { DisclosurePanel as Primitive } from "@headlessui/react";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new collapsible content element.
 * @param props Configurable options for the element.
 * @returns A collapsible content element.
 */
export const CollapsibleContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};
