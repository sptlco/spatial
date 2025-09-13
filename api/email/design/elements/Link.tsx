// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Link as Primitive } from "@react-email/components";

import { Element, ElementProps, Node } from ".";

/**
 * Create a new link element.
 * @param props Configurable options for the element.
 * @returns A link element.
 */
export const Link: Element<LinkProps> = (props: LinkProps): Node => {
  return (
    <Primitive
      href={props.href}
      style={props.style}
      className={clsx("text-foreground-accent text-inherit", props.className)}
      children={props.children}
    />
  );
};

type LinkProps = ElementProps & {
  href?: string;
};
