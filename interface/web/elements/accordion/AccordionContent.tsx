// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";
import { DisclosurePanel as Primitive } from "@headlessui/react";

/**
 * Create a new accordion content element.
 * @param props Configurable options for the element.
 * @returns An accordion content element.
 */
export const AccordionContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      transition
      style={props.style}
      className={clsx(
        "pb-1u",
        "origin-top transition-all duration-200 ease-out",
        "data-[closed]:-translate-y-6 data-[closed]:opacity-0",
        "text-foreground-secondary",
        props.className,
      )}
      children={props.children}
    />
  );
};
