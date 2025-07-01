// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";
import { Disclosure as Primitive } from "@headlessui/react";

/**
 * Create a new accordion item element.
 * @param props Configurable options for the element.
 * @returns An accordion item element.
 */
export const AccordionItem: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      as="div"
      style={props.style}
      className={clsx(
        "flex flex-col",
        "border-border-bounds border-0 border-b",
        props.className,
      )}
      children={props.children}
    />
  );
};
