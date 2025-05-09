// Copyright © Spatial. All rights reserved.

import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new alert dialog message element.
 * @param props Configurable options for the element.
 * @returns An alert dialog message element.
 */
export const AlertDialogMessage: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className="flex flex-col"
      children={props.children}
    />
  );
};
