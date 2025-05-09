// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H3 element.
 * @param props Configurable options for the element.
 * @returns An H3 element.
 */
export const H3: Element = (props: ElementProps): Node => {
  return (
    <h3
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
