// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new table caption element.
 * @param props Configurable options for the element.
 * @returns A table caption element.
 */
export const TableCaption: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("text-s text-foreground-secondary", props.className)}
    />
  );
};
