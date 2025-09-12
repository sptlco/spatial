// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";

import { Div, Element, ElementProps, Node, ScrollArea } from "..";

/**
 * Create a new page element.
 * @param props Configurable options for the element.
 * @returns A page element.
 */
export const Page: Element = (props: ElementProps): Node => {
  return (
    <main
      vaul-drawer-wrapper=""
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={clsx(
        "grow",
        "min-h-full",
        "flex flex-col",
        "bg-background-primary",
        "text-foreground-primary",
        props.className,
      )}
    />
  );
};
