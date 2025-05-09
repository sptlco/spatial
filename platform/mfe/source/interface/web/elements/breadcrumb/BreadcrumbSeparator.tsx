// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Icon, Node } from "..";

/**
 * Create a new breadcrumb separator element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb separator element.
 */
export const BreadcrumbSeparator: Element = (props: ElementProps): Node => {
  return (
    <Icon
      ref={props.ref}
      style={props.style}
      className={clsx("text-foreground-quaternary", props.className)}
      icon="chevron_right"
    />
  );
};
