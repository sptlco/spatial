// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, Icon, NavigatorTriggerProps, Node, Span } from "../..";
import clsx from "clsx";

/**
 * Create a new navigator trigger element.
 * @param props Configurable options for the element.
 * @returns A navigator trigger element.
 */
export const NavigatorTrigger: Element<NavigatorTriggerProps> = (
  props: NavigatorTriggerProps,
): Node => {
  let augmentedProps: Primitive.NavigationMenuTriggerProps = {};

  if (props.suppressEvents) {
    augmentedProps = {
      onPointerMove: (e) => e.preventDefault(),
      onPointerLeave: (e) => e.preventDefault(),
    };
  }

  return (
    <Primitive.Trigger
      ref={props.ref}
      asChild={props.asChild}
      style={props.style}
      disabled={props.disabled}
      {...augmentedProps}
      className={clsx(
        "group",
        "transition-all",
        "!outline-none",
        "space-x-1/2u inline-flex items-center justify-center",
        "disabled:pointer-events-none disabled:opacity-50",
        "text-foreground-tertiary",
        "hover:text-foreground-primary",
        "data-[state=open]:text-foreground-primary",
        props.className,
      )}
    >
      <Span className="grow">{props.children}</Span>
      <Icon
        icon="chevron_right"
        className={clsx(
          "relative",
          "transition-all",
          "rotate-90 group-data-[state=open]:-rotate-90",
        )}
      />
    </Primitive.Trigger>
  );
};
