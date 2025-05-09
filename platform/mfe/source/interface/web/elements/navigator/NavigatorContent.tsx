// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

type NavigatorContentProps = ElementProps & {
  suppressEvents?: boolean;
};

/**
 * Create a new navigator content element.
 * @param props Configurable options for the element.
 * @returns A navigator content element.
 */
export const NavigatorContent: Element<NavigatorContentProps> = (
  props: NavigatorContentProps,
): Node => {
  let augmentedProps: Primitive.NavigationMenuContentProps = {};

  if (props.suppressEvents) {
    augmentedProps = {
      onPointerMove: (e) => e.preventDefault(),
      onFocusOutside: (e) => e.preventDefault(),
      onInteractOutside: (e) => e.preventDefault(),
      onEscapeKeyDown: (e) => e.preventDefault(),
      onPointerDownOutside: (e) => e.preventDefault(),
      onPointerLeave: (e) => e.preventDefault(),
    };
  }

  return (
    <Primitive.Content
      ref={props.ref}
      style={props.style}
      children={props.children}
      {...augmentedProps}
      className={clsx(
        "transition-all",
        "data-[orientation=horizontal]:p-3/2u",
        "top-0 data-[orientation=horizontal]:absolute",
        "data-[orientation=horizontal]:data-[motion^=from-]:animate-in data-[orientation=horizontal]:data-[motion^=to-]:animate-out",
        "data-[orientation=horizontal]:data-[motion^=from-]:fade-in data-[orientation=horizontal]:data-[motion^=to-]:fade-out",
        "data-[orientation=horizontal]:data-[motion=from-end]:slide-in-from-right-4u data-[orientation=horizontal]:data-[motion=from-start]:slide-in-from-left-4u",
        "data-[orientation=horizontal]:data-[motion=to-end]:slide-out-to-right-4u data-[orientation=horizontal]:data-[motion=to-start]:slide-out-to-left-4u",
        props.className,
      )}
    />
  );
};
