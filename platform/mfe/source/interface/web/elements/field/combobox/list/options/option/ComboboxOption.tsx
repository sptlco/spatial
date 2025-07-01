// Copyright © Spatial Corporation. All rights reserved.

import { ComboboxOption as Primitive } from "@headlessui/react";
import { ComboboxOptionProps, Element, Icon, Node, Span } from "../../../../..";
import clsx from "clsx";

/**
 * Create a new combobox option element.
 * @param props Configurable options for the element.
 * @returns A combobox option element.
 */
export const ComboboxOption: Element<ComboboxOptionProps> = (
  props: ComboboxOptionProps,
): Node => {
  return (
    <Primitive
      key={props.data.key}
      value={props.data.value}
      className={clsx(
        "transition-all",
        "cursor-pointer",
        "rounded-1/2u",
        "px-1u py-1/2u space-x-1u group flex w-full items-center",
        "hover:bg-background-secondary",
        "data-[focus]:bg-background-secondary",
        "data-[active]:bg-background-secondary",
        "data-[selected]:!bg-background-control-selected-default",
        "data-[selected]:text-base-white-9",
      )}
    >
      <Icon
        icon="check"
        className="invisible transition-all group-data-[selected]:visible"
      />
      <Span className="grow transition-all">{props.data.value}</Span>
    </Primitive>
  );
};
