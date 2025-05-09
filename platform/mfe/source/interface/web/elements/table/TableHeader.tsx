// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, TableRow } from "..";

/**
 * Create a new table header element.
 * @param props Configurable options for the element.
 * @returns A table header element.
 */
export const TableHeader: Element = (props: ElementProps): Node => {
  return (
    <thead
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
    >
      <TableRow children={props.children} />
    </thead>
  );
};
