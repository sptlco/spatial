// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new table content element.
 * @param props Configurable options for the element.
 * @returns A table content element.
 */
export const TableContent: Element = (props: ElementProps): Node => {
  return (
    <table
      ref={props.ref}
      style={props.style}
      children={props.children}
      align="left"
      className={clsx(
        "w-full",
        "table-fixed",
        "border-collapse",
        "rounded-1/2u overflow-hidden",
        "divide-border-bounds divide-y-[1px]",
        props.className,
      )}
    />
  );
};
