// Copyright © Spatial. All rights reserved.

import { Element, ElementProps, Icon, Node } from "..";

/**
 * Create a new pagination ellipsis element.
 * @param props Configurable options for the element.
 * @returns A pagination ellipsis element.
 */
export const PaginationEllipsis: Element = (props: ElementProps): Node => {
  return (
    <Icon
      ref={props.ref}
      style={props.style}
      className={props.className}
      icon="more_horiz"
    />
  );
};
