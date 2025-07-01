// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { A, AProps, Element, Node } from "..";

/**
 * Create a new breadcrumb link element.
 * @param props Configurable options for the element.
 * @returns A breadcrumb link element.
 */
export const BreadcrumbLink: Element<AProps> = (props: AProps): Node => {
  return (
    <A
      ref={props.ref}
      href={props.href}
      onClick={props.onClick}
      style={props.style}
      className={clsx(
        "transition-all",
        "h-fit w-fit",
        "text-foreground-quaternary",
        "hover:text-foreground-primary",
        props.className,
      )}
      children={props.children}
    />
  );
};
