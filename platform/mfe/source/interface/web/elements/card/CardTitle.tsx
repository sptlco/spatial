// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new card title element.
 * @param props Configurable options for the element.
 * @returns A card title element.
 */
export const CardTitle: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      className={clsx("text-l font-bold", props.className)}
      children={props.children}
    />
  );
};
