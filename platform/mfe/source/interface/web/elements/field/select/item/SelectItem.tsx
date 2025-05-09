// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-select";
import { Element, Icon, Node, SelectItemProps, Span } from "../../..";
import clsx from "clsx";

/**
 * Create a new select item element.
 * @param props Configurable options for the element.
 * @returns A select item element.
 */
export const SelectItem: Element<SelectItemProps> = (
  props: SelectItemProps,
): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      style={props.style}
      value={props.value}
      className={clsx(
        "transition-all",
        "cursor-pointer",
        "rounded-1/2u !outline-none",
        "space-x-1u px-1u py-1/2u flex items-center",
        "data-[highlighted]:bg-background-tertiary",
        "data-[state=checked]:!bg-background-control-selected-default",
        "data-[state=checked]:!text-base-white-9",
        "data-[disabled]:opacity-50",
        "data-[disabled]:pointer-events-none",
        props.className,
      )}
    >
      <Span className="size-3/2u flex items-center justify-center">
        <Primitive.ItemIndicator className="size-3/2u flex items-center justify-center">
          <Icon icon="check" />
        </Primitive.ItemIndicator>
      </Span>
      <Primitive.ItemText>{props.children}</Primitive.ItemText>
    </Primitive.Item>
  );
};
