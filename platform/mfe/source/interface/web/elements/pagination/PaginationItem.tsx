// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, LI, Node } from "..";

/**
 * Create a new pagination item element.
 * @param props Configurable options for the element.
 * @returns A pagination item element.
 */
export const PaginationItem: Element = (props: ElementProps): Node => {
  return (
    <LI
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "inline-flex items-center justify-center",
        props.className,
      )}
    />
  );
};
