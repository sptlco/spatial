// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new card footer element.
 * @param props Configurable options for the element.
 * @returns A card footer element.
 */
export const CardFooter: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex", props.className)}
      children={props.children}
    />
  );
};
