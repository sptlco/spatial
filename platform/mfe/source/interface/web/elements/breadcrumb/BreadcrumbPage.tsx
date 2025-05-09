// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new breadcrumb page element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb page element.
 */
export const BreadcrumbPage: Element = (props: ElementProps): Node => {
  return (
    <Span
      ref={props.ref}
      style={props.style}
      className={clsx(props.className)}
      children={props.children}
    />
  );
};
