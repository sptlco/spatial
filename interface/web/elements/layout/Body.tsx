// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new body element.
 * @param props Configurable options for the element.
 * @returns A body element.
 */
export const Body: Element = (props: ElementProps): Node => {
  return (
    <body
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "h-fit",
        "flex flex-col",
        "min-h-screen",
        "bg-background-primary",
        "text-foreground-primary",
        "font-regular text-m font-sans",
        props.className,
      )}
    />
  );
};
