// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import {
  ComboboxListProps,
  ComboboxOptions,
  ComboboxSearch,
  Div,
  Element,
  Node,
} from "../../..";

/**
 * Create a new combobox list element.
 * @param props Configurable options for the element.
 * @returns A combobox list element.
 */
export const ComboboxList: Element<ComboboxListProps> = (
  props: ComboboxListProps,
): Node => {
  return (
    <Div
      className={clsx(
        "transition-all",
        "rounded-1/2u",
        "bg-background-primary",
        "border-border-control-default border",
        "absolute left-0 top-full z-20 w-full",
        "invisible scale-95 group-data-[open]:visible group-data-[open]:scale-100",
      )}
    >
      <ComboboxSearch
        query={props.query}
        setQuery={props.setQuery}
        placeholder={props.searchPlaceholder}
      />
      <ComboboxOptions query={props.query} options={props.options} />
    </Div>
  );
};
