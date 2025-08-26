// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new form footer element.
 * @param props Configurable options for the element.
 * @returns A form footer element.
 */
export const FormFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("flex", props.className)}
    />
  );
};
