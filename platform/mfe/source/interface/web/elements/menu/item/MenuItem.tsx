// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { MenuItemProps, Element, Icon, Node, Span } from "../..";
import clsx from "clsx";

/**
 * Create a new menu item element.
 * @param props Configurable options for the element.
 * @returns A menu item element.
 */
export const MenuItem: Element<MenuItemProps> = ({
  inset = true,
  ...props
}: MenuItemProps): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      style={props.style}
      onSelect={props.onSelect}
      disabled={props.disabled}
      className={clsx(
        "group",
        "cursor-pointer transition-all",
        "rounded-1/2u !outline-none",
        "space-x-1/2u px-1/2u py-1/4u flex items-center",
        "data-[disabled]:pointer-events-none data-[disabled]:opacity-50",
        "data-[highlighted]:bg-background-accent",
        "data-[highlighted]:text-base-white-9",
        props.className,
      )}
    >
      {(inset || props.icon) && (
        <Span className="size-3/2u flex items-center justify-center">
          {props.icon && <Icon icon={props.icon} />}
        </Span>
      )}
      <Span className="grow">{props.children}</Span>
      {props.shortcut && (
        <Span
          className={clsx(
            "transition-all",
            "text-foreground-tertiary",
            "group-data-[highlighted]:text-base-white-9",
            "h-3/2u min-w-3/2u px-1/4u text-s flex items-center justify-center",
          )}
        >
          {props.shortcut}
        </Span>
      )}
    </Primitive.Item>
  );
};
