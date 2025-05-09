// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new card header element.
 * @param props Configurable options for the element.
 * @returns A card header element.
 */
export const CardHeader: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex flex-col", props.className)}
      children={props.children}
    />
  );
};
