// Copyright © Spatial Corporation. All rights reserved.

import { ComboboxButton as Primitive } from "@headlessui/react";
import { ComboboxButtonProps, Element, Icon, Node, Span } from "../../..";
import clsx from "clsx";

/**
 * Create a new combobox button element.
 * @param props Configurable options for the element.
 * @returns A combobox button element.
 */
export const ComboboxButton: Element<ComboboxButtonProps> = (
  props: ComboboxButtonProps,
): Node => {
  return (
    <Primitive
      className={clsx(
        "group cursor-pointer transition-all group-data-[disabled]:pointer-events-none",
        "space-x-1/2u px-1u py-1/2u rounded-1/2u flex w-full items-center",
        "bg-background-control-default outline-border-control-default outline outline-1",
        "data-[active]:outline-2 data-[open]:outline-2",
        "hover:outline-border-control-hover data-[open]:outline-border-control-focus data-[active]:outline-border-control-active",
      )}
    >
      <Span
        className={clsx("grow whitespace-nowrap text-left transition-all", {
          "text-foreground-quaternary": !props.value,
        })}
      >
        {props.value || props.placeholder}
      </Span>
      <Icon
        icon="unfold_more"
        className={clsx(
          "transition-all",
          "group-data-[active]:text-border-control-active",
          "group-data-[open]:text-border-control-focus",
        )}
      />
    </Primitive>
  );
};
