// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import {
  MenuCheckboxItemProps,
  Element,
  Icon,
  Node,
  Span,
} from "@spatial/elements";
import clsx from "clsx";

/**
 * Create a new menu checkbox item element.
 * @param props Configurable options for the element.
 * @returns A menu checkbox item element.
 */
export const MenuCheckboxItem: Element<MenuCheckboxItemProps> = (
  props: MenuCheckboxItemProps,
): Node => {
  return (
    <Primitive.CheckboxItem
      ref={props.ref}
      style={props.style}
      checked={props.checked}
      defaultChecked={props.defaultChecked}
      onCheckedChange={props.onChange}
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
      <Span className="size-3/2u flex items-center justify-center">
        <Primitive.ItemIndicator className="flex items-center justify-center">
          <Icon className="text-foreground-primary" icon="check" />
        </Primitive.ItemIndicator>
      </Span>
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
    </Primitive.CheckboxItem>
  );
};
