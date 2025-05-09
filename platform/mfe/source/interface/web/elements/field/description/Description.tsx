// Copyright © Spatial. All rights reserved.

import {Description as Primitive} from "@headlessui/react";
import {Element, ElementProps, Node} from "../..";
import clsx from "clsx";

/**
 * Create a new description element.
 * @param props Configurable options for the element.
 * @returns A description element.
 */
export const Description: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "w-fit",
        "transition-all",
        "text-s text-foreground-secondary",
        "data-[disabled]:cursor-not-allowed",
        props.className,
      )}
    />
  );
};
