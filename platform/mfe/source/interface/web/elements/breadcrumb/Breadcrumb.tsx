// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new breadcrumb element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb element.
 */
export const Breadcrumb: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("flex h-fit w-full flex-wrap items-center")}
      children={props.children}
    />
  );
};
