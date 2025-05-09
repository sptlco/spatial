// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, UL } from "..";

/**
 * Create a new pagination content element.
 * @param props Configurable options for the element.
 * @returns A pagination content element.
 */
export const PaginationContent: Element = (props: ElementProps): Node => {
  return (
    <UL
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("gap-1/2u flex items-center", props.className)}
    />
  );
};
