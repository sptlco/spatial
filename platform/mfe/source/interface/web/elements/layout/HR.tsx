// Copyright © Spatial. All rights reserved.

import { Element, ElementProps, Node } from "..";

/**
 * Create a new HR element.
 * @param props Configurable options for the element.
 * @returns A HR element.
 */
export const HR: Element = (props: ElementProps): Node => {
  return (
    <hr
      ref={props.ref}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};
