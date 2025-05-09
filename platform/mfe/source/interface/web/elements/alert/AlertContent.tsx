// Copyright © Spatial. All rights reserved.

import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new alert content element.
 * @param props Configurable options for the element.
 * @returns An alert content element.
 */
export const AlertContent: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className="flex h-fit grow flex-col"
      children={props.children}
    />
  );
};
