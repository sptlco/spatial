// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new checklist element.
 * @param props Configurable options for the element.
 * @returns A checklist element.
 */
export const Checklist: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("space-y-1u flex flex-col", props.className)}
      children={props.children}
    />
  );
};
