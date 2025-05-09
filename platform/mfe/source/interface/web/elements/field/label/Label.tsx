// Copyright © Spatial. All rights reserved.

import { Label as Primitive } from "@headlessui/react";
import { Element, ElementProps, Node, Span } from "../..";
import clsx from "clsx";

/**
 * Create a new label element.
 * @param props Configurable options for the element.
 * @returns A label element.
 */
export const Label: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      style={props.style}
      className={clsx(
        "text-s font-bold",
        "transition-all",
        "group-focus-within/field:!text-border-control-focus",
        "space-x-1/2u flex w-fit items-center",
        "data-[disabled]:cursor-not-allowed",
        props.className,
      )}
    >
      <Span className="grow">{props.children}</Span>
    </Primitive>
  );
};
