// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new breadcrumb item element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb item element.
 */
export const BreadcrumbItem: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx(
        "flex h-fit w-fit items-center text-ellipsis whitespace-nowrap",
        props.className,
      )}
      children={props.children}
    />
  );
};
