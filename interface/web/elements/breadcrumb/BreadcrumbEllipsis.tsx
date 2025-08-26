// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Icon, Node } from "..";

/**
 * Create a new breadcrumb ellipsis element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb ellipsis element.
 */
export const BreadcrumbEllipsis: Element = (props: ElementProps): Node => {
  return (
    <Icon
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-quaternary", props.className)}
      icon="more_horiz"
    />
  );
};
