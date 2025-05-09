// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new sheet footer element.
 * @param props Configurable options for the element.
 * @returns A sheet footer element.
 */
export const SheetFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex", props.className)}
      children={props.children}
    />
  );
};
