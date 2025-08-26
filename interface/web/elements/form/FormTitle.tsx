// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new form title element.
 * @param props Configurable options for the element.
 * @returns A form title element.
 */
export const FormTitle: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("text-l font-bold", props.className)}
    />
  );
};
