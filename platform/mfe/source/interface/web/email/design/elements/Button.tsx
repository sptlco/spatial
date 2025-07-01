// Copyright © Spatial Corporation. All rights reserved.

import { Button as Primitive } from "@react-email/components";
import clsx from "clsx";

import { Element, ElementProps, Node } from ".";

/**
 * Create a new button element.
 * @param props Configurable options for the element.
 * @returns A button element.
 */
export const Button: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      className={clsx(
        "rounded-1/2u bg-background-primary max-h-5/2u py-1/2u px-1u",
        props.className,
      )}
      children={props.children}
    />
  );
};
