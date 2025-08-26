// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new form description element.
 * @param props Configurable options for the element.
 * @returns A form description element.
 */
export const FormDescription: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("text-foreground-secondary", props.className)}
    />
  );
};
