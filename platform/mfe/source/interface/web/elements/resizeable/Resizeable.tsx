// Copyright © Spatial. All rights reserved.

"use client";

import { Panel as Primitive } from "react-resizable-panels";
import { Element, Node, ResizeableProps } from "..";

/**
 * Create a new resizeable panel element.
 * @param props Configurable options for the element.
 * @returns A resizeable panel element.
 */
export const Resizeable: Element<ResizeableProps> = (
  props: ResizeableProps,
): Node => {
  return (
    <Primitive
      ref={props.ref}
      defaultSize={props.defaultSize}
      minSize={props.minSize}
      maxSize={props.maxSize}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};
