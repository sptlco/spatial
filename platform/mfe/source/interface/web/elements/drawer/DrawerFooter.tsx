// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new drawer footer element.
 * @param props Configurable options for the element.
 * @returns A drawer footer element.
 */
export const DrawerFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex", props.className)}
      children={props.children}
    />
  );
};
