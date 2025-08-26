// Copyright © Spatial Corporation. All rights reserved.

import { ComboboxInput as Primitive } from "@headlessui/react";
import { ComboboxSearchProps, Div, Element, Icon, Node } from "../../../..";
import clsx from "clsx";

/**
 * Create a new combobox search element.
 * @param props Configurable options for the element.
 * @returns A combobox search element.
 */
export const ComboboxSearch: Element<ComboboxSearchProps> = ({
  placeholder = "Search...",
  ...props
}: ComboboxSearchProps): Node => {
  return (
    <Div className="p-1/2u w-full">
      <Div className="space-x-1u px-1u py-1/2u flex items-center">
        <Icon icon="search" />
        <Primitive
          value={props.query}
          onChange={(e) => props.setQuery(e.target.value)}
          placeholder={placeholder}
          className={clsx(
            "grow",
            "placeholder:text-foreground-quaternary bg-transparent",
            "focus-visible:outline-none",
          )}
        />
      </Div>
    </Div>
  );
};
