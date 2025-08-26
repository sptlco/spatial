// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node, TableCaption, TableProps } from "..";

/**
 * Create a new table element.
 * @param props Configurable options for the element.
 * @returns A table element.
 */
export const Table: Element<TableProps> = (props: TableProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "inline-flex w-full flex-col",
        "space-y-1u",
        props.className,
      )}
    />
  );
};
