// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new alert dialog footer element.
 * @param props Configurable options for the element.
 * @returns An alert dialog footer element.
 */
export const AlertDialogFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("space-x-1/2u flex justify-end", props.className)}
      children={props.children}
    />
  );
};
