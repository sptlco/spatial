// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new table row element.
 * @param props Configurable options for the element.
 * @returns A table row element.
 */
export const TableRow: Element = (props: ElementProps): Node => {
  return (
    <tr
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "transition-all",
        "hover:bg-background-secondary",
        props.className,
      )}
    />
  );
};
