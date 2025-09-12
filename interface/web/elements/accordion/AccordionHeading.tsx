// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Icon, Node, Span } from "..";
import { DisclosureButton as Primitive } from "@headlessui/react";

/**
 * Create a new accordion heading element.
 * @param props Configurable options for the element.
 * @returns An accordion heading element.
 */
export const AccordionHeading: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      style={props.style}
      className={clsx(
        "py-1u space-x-1u group flex items-center font-bold",
        props.className,
      )}
    >
      <Span className="flex grow text-left">{props.children}</Span>
      <Icon
        className="transition-all duration-200 group-data-[open]:rotate-180"
        icon="keyboard_arrow_down"
      />
    </Primitive>
  );
};
