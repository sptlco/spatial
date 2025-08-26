// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new skeleton element.
 * @param props Configurable options for the element.
 * @returns A skeleton element.
 */
export const Skeleton: Element = (props: ElementProps): Node => {
  return (
    <Div
      children={props.children}
      className={clsx(
        "animate-pulse",
        "rounded-1/2u",
        "bg-background-translucent-primary",
        props.className,
      )}
    />
  );
};
