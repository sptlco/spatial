// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H4 element.
 * @param props Configurable options for the element.
 * @returns An H4 element.
 */
export const H4: Element = (props: ElementProps): Node => {
  return (
    <h4
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
