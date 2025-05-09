// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new dialog footer element.
 * @param props Configurable options for the element.
 * @returns A dialog footer element.
 */
export const DialogFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("space-x-1/2u flex justify-end", props.className)}
      children={props.children}
    />
  );
};
