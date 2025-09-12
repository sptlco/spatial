// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Combobox as Primitive } from "@headlessui/react";
import {
  ComboboxButton,
  ComboboxList,
  ComboboxProps,
  Element,
  Field,
  Node,
} from "../..";
import { useState } from "react";
import clsx from "clsx";

/**
 * Create a new combobox element.
 * @param props Configurable options for the element.
 * @returns A combobox element.
 */
export const Combobox: Element<ComboboxProps> = ({
  placeholder = "Select...",
  ...props
}: ComboboxProps): Node => {
  const [query, setQuery] = useState("");

  return (
    <Field
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={props.className}
    >
      <Primitive
        as="div"
        ref={props.ref}
        name={props.name}
        value={props.value}
        onChange={props.onChange}
        onClose={() => setQuery("")}
        style={props.style}
        className={clsx(
          "data-[disabled]:opacity-50",
          "space-y-1/2u group relative flex w-full flex-col",
        )}
      >
        <ComboboxButton value={props.value} placeholder={placeholder} />
        <ComboboxList
          query={query}
          setQuery={setQuery}
          searchPlaceholder={props.searchPlaceholder}
          options={props.options}
        />
      </Primitive>
    </Field>
  );
};
