// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new card description element.
 * @param props Configurable options for the element.
 * @returns A card description element.
 */
export const CardDescription: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      className={clsx("text-m text-foreground-secondary", props.className)}
      children={props.children}
    />
  );
};
