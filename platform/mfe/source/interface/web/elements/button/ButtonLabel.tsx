// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new button label element.
 * @param props Configurable options for the element.
 * @returns A button label element.
 */
export const ButtonLabel: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "space-x-1/2u pointer-events-none flex h-fit w-fit items-center",
        props.className,
      )}
    />
  );
};
