// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { A, Element, Node, PaginationLinkProps } from "../..";

/**
 * Create a new pagination link element.
 * @param props Configurable options for the element.
 * @returns A pagination link element.
 */
export const PaginationLink: Element<PaginationLinkProps> = (
  props: PaginationLinkProps,
): Node => {
  return (
    <A
      ref={props.ref}
      style={props.style}
      href={props.href}
      children={props.children}
      className={clsx(
        "rounded-1/2u",
        "transition-all",
        "p-1/4u px-1/2u min-w-2u",
        "hover:bg-background-secondary",
        "relative flex shrink-0 items-center justify-center whitespace-nowrap",
        {
          "!bg-background-interactive-primary-default": props.active,
          "text-foreground-inverted-primary": props.active,
        },
        props.className,
      )}
    />
  );
};
