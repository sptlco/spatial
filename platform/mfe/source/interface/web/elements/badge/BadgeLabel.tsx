// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new badge label element.
 * @param props Configurable options for the badge label.
 * @returns A badge label element.
 */
export const BadgeLabel: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "space-x-1/2u flex h-fit w-fit items-center",
        props.className,
      )}
    />
  );
};
