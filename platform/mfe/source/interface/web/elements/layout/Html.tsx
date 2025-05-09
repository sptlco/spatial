// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element } from "../Element";
import { ElementProps } from "../ElementProps";
import { Node } from "../Node";

/**
 * Create a new HTML element.
 * @param props Configurable options for the element.
 * @returns An HTML element.
 */
export const Html: Element = (props: ElementProps): Node => {
  return (
    <html
      lang="en"
      ref={props.ref}
      style={props.style}
      children={props.children}
      suppressHydrationWarning
      className={clsx(
        "group/html",
        "h-fit",
        "min-h-screen",
        "bg-background-primary text-foreground-primary",
        props.className,
      )}
    />
  );
};
