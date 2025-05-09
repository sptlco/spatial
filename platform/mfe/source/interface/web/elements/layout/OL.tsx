// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new OL element.
 * @param props Configurable options for the element.
 * @returns A OL element.
 */
export const OL: Element = (props: ElementProps): Node => {
  return (
    <ol
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
