// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new card element.
 * @param props Configurable options for the element.
 * @returns A card element.
 */
export const Card: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx(
        "space-y-3/2u flex h-fit w-full shrink flex-col",
        props.className,
      )}
      children={props.children}
    />
  );
};
