// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new accordion element.
 * @param props Configurable options for the element.
 * @returns An accordion element.
 */
export const Accordion: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex w-full flex-col", props.className)}
      children={props.children}
    />
  );
};
