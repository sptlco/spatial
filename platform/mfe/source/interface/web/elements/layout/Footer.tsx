// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new footer element.
 * @param props Configurable options for the element.
 * @returns A footer element.
 */
export const Footer: Element = (props: ElementProps): Node => {
  return (
    <footer
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx("!mt-auto w-full", props.className)}
    />
  );
};
