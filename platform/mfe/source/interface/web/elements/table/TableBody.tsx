// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new table body element.
 * @param props Configurable options for the element.
 * @returns A table body element.
 */
export const TableBody: Element = (props: ElementProps): Node => {
  return (
    <tbody
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("divide-border-bounds divide-y-[1px]", props.className)}
    />
  );
};
