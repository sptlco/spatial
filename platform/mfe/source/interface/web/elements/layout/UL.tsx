// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new UL element.
 * @param props Configurable options for the element.
 * @returns A UL element.
 */
export const UL: Element = (props: ElementProps): Node => {
  return (
    <ul
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
