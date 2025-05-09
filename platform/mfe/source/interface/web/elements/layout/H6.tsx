// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H6 element.
 * @param props Configurable options for the element.
 * @returns An H6 element.
 */
export const H6: Element = (props: ElementProps): Node => {
  return (
    <h6
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
