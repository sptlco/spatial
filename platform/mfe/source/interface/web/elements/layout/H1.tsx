// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new H1 element.
 * @param props Configurable options for the element.
 * @returns An H1 element.
 */
export const H1: Element = (props: ElementProps): Node => {
  return (
    <h1
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
