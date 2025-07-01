// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";

import { Container, Element, ElementProps, Node } from "..";

/**
 * Create a new body element.
 * @param props Configurable options for the element.
 * @returns A body element.
 */
export const Body: Element = (props: ElementProps): Node => {
  return (
    <Container
      children={props.children}
      className={clsx(
        "mx-auto",
        "max-w-32u w-full",
        "rounded-1u bg-background-primary",
        "border-border-bounds border-1 border border-solid",
        props.className,
      )}
    />
  );
};
