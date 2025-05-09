// Copyright © Spatial. All rights reserved.

import { Element, ElementProps, Node, Span } from "..";

/**
 * Create a new alert description element.
 * @param props Configurable options for the element.
 * @returns An alert description element.
 */
export const AlertDescription: Element = (props: ElementProps): Node => {
  return <Span ref={props.ref} style={props.style} children={props.children} />;
};
