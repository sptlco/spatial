// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { A, Element, NavigatorLinkProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new navigator link element.
 * @param props Configurable options for the element.
 * @returns A navigator link element.
 */
export const NavigatorLink: Element<NavigatorLinkProps> = (
  props: NavigatorLinkProps,
): Node => {
  return (
    <Primitive.Link asChild>
      <A
        ref={props.ref}
        href={props.href}
        style={props.style}
        children={props.children}
        className={clsx(
          "group",
          "size-full",
          "transition-all",
          "!outline-none",
          "space-x-1/2u inline-flex items-center",
          "text-foreground-tertiary",
          "data-[active]:text-foreground-primary",
          "hover:text-foreground-primary focus:text-foreground-primary",
          props.className,
        )}
      />
    </Primitive.Link>
  );
};
