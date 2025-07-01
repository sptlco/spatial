// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { AProps, Element, Icon, Node, Span } from "../..";

/**
 * Create a new anchor element.
 * @param props Configurable options for the element.
 * @returns An anchor element.
 */
export const A: Element<AProps> = (props: AProps): Node => {
  return (
    <a
      suppressHydrationWarning
      ref={props.ref}
      href={props.href}
      onClick={props.onClick}
      style={props.style}
      target={props.external ? "_blank" : undefined}
      className={clsx(
        "cursor-pointer",
        "space-x-1/4u inline-flex items-center",
        { "text-foreground-accent": props.highlight },
        props.className,
      )}
    >
      {props.external ? <Span>{props.children}</Span> : props.children}
      {props.external && <Icon icon="open_in_new" className="!text-inherit" />}
    </a>
  );
};
