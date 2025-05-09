// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new table column element.
 * @param props Configurable options for the element.
 * @returns A table column element.
 */
export const TableColumn: Element = (props: ElementProps): Node => {
  return (
    <th
      ref={props.ref}
      style={props.style}
      align="left"
      children={props.children}
      className={clsx(
        "py-1/2u px-3/4u",
        "font-regular text-foreground-secondary",
        props.className,
      )}
    />
  );
};
