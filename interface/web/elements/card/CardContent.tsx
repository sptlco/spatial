// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new card content element.
 * @param props Configurable options for the element.
 * @returns A card content element.
 */
export const CardContent: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex w-full", props.className)}
      children={props.children}
    />
  );
};
