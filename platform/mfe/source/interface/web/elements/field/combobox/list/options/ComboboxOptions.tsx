// Copyright © Spatial. All rights reserved.

import { ComboboxOptions as Primitive } from "@headlessui/react";
import {
  ComboboxOption,
  ComboboxOptionsProps,
  Element,
  IComboboxOption,
  Node,
  Span,
} from "../../../..";
import clsx from "clsx";

/**
 * Create a new combobox options element.
 * @param props Configurable options for the element.
 * @returns A combobox options element.
 */
export const ComboboxOptions: Element<ComboboxOptionsProps> = (
  props: ComboboxOptionsProps,
): Node => {
  const options = (): IComboboxOption[] => {
    if (props.query) {
      const query = props.query.toLowerCase();
      return props.options.filter((opt) =>
        opt.value.toLowerCase().match(query),
      );
    }

    return props.options;
  };

  const list = options();

  return (
    <Primitive
      transition
      className={clsx(
        "p-1/2u space-y-1/2u border-t-border-control-default flex w-full flex-col border-t",
        "transition-all data-[leave]:data-[closed]:opacity-0",
      )}
    >
      {list.length > 0 ? (
        list.map((opt, i) => <ComboboxOption key={i} data={opt} />)
      ) : (
        <Span className="text-s text-center">No results found</Span>
      )}
    </Primitive>
  );
};
