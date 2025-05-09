// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new table cell element.
 * @param props Configurable options for the element.
 * @returns A table cell element.
 */
export const TableCell: Element = (props: ElementProps): Node => {
  return (
    <td
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "overflow-hidden",
        "py-1/2u px-3/4u",
        "text-ellipsis whitespace-nowrap",
        props.className,
      )}
    />
  );
};
