// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-select";
import { clsx } from "clsx";
import { createContext, useContext } from "react";

const Context = createContext<{ multiple?: boolean }>({});

/**
 * Displays a list of options for the user to pick from.
 */
export const Select: typeof Primitive = {
  ...Primitive,

  /**
   * Contains all the parts of a select menu.
   */
  Root: createElement<typeof Primitive.Root, { multiple?: boolean }>(({ multiple, ...props }, _) => (
    <Context.Provider value={{ multiple }}>
      <Primitive.Root {...props} />
    </Context.Provider>
  )),

  /**
   * Opens the select menu.
   */
  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "data-disabled:opacity-50",
        "w-full px-4 py-2 bg-input rounded-lg transition-all",
        "outline-2 outline-offset-3 outline-transparent focus:outline-line-input-focus focus-within:outline-line-input-focus",
        props.className
      )}
    />
  )),

  /**
   * The component that pops out when the select menu is open.
   */
  Content: createElement<typeof Primitive.Content>((props, ref) => (
    <Primitive.Portal>
      <Primitive.Content {...props} ref={ref}>
        {/** */}
      </Primitive.Content>
    </Primitive.Portal>
  ))
};
