// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new LI element.
 * @param props Configurable options for the element.
 * @returns A LI element.
 */
export const LI: Element = (props: ElementProps): Node => {
  return (
    <li
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
