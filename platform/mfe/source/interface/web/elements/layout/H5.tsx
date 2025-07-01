// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H5 element.
 * @param props Configurable options for the element.
 * @returns An H5 element.
 */
export const H5: Element = (props: ElementProps): Node => {
  return (
    <h5
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
