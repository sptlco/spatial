// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H2 element.
 * @param props Configurable options for the element.
 * @returns An H2 element.
 */
export const H2: Element = (props: ElementProps): Node => {
  return (
    <h2
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
