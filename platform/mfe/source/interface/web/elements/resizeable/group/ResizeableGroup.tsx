// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { PanelGroup as Primitive } from "react-resizable-panels";
import { Element, Node, ResizeableGroupProps } from "../..";
import clsx from "clsx";

/**
 * Create a new resizeable group element.
 * @param props Configurable options for the element.
 * @returns A resizeable group element.
 */
export const ResizeableGroup: Element<ResizeableGroupProps> = ({
  direction = "horizontal",
  ...props
}: ResizeableGroupProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      direction={direction}
      style={props.style}
      children={props.children}
      className={clsx(
        "flex size-full",
        "data-[panel-group-direction=vertical]:flex-col",
        props.className,
      )}
    />
  );
};
