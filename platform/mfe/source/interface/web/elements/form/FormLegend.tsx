// Copyright © Spatial. All rights reserved.

import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new form legend element.
 * @param props Configurable options for the element.
 * @returns A form legend element.
 */
export const FormLegend: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className="flex w-full flex-col"
      children={props.children}
    />
  );
};
