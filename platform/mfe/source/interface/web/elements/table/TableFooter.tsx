// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, TableRow } from "..";

/**
 * Create a new table footer element.
 * @param props Configurable options for the element.
 * @returns A table footer element.
 */
export const TableFooter: Element = (props: ElementProps): Node => {
  return (
    <tfoot
      ref={props.ref}
      style={props.style}
      className={clsx("bg-background-secondary font-bold", props.className)}
    >
      <TableRow children={props.children} />
    </tfoot>
  );
};
