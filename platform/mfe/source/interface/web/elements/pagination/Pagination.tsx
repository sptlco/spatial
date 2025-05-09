// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new pagination element.
 * @param props Configurable options for the element.
 * @returns A pagination element.
 */
export const Pagination: Element = (props: ElementProps): Node => {
  return (
    <nav
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("mx-auto flex w-full justify-center", props.className)}
    />
  );
};
