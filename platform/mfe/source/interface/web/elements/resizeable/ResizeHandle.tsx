// Copyright © Spatial. All rights reserved.

"use client";

import { PanelResizeHandle as Primitive } from "react-resizable-panels";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new resize handle element.
 * @param props Configurable options for the element.
 * @returns A resize handle element.
 */
export const ResizeHandle: Element = (props: ElementProps): Node => {
  return (
    <Primitive
      style={props.style}
      children={props.children}
      className={clsx(
        "relative",
        "bg-border-bounds",
        "flex items-center justify-center",
        "data-[panel-group-direction=vertical]:h-[1px]",
        "data-[panel-group-direction=horizontal]:w-[1px]",
        props.className,
      )}
    />
  );
};
