// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toggle-group";
import {
  Element,
  Node,
  ToggleGroupMultipleProps,
  ToggleGroupSingleProps,
} from "../..";
import clsx from "clsx";

/**
 * Create a new toggle group element.
 * @param props Configurable options for the element.
 * @returns A toggle group element.
 */
export const ToggleGroup: Element<
  ToggleGroupSingleProps | ToggleGroupMultipleProps
> = (props: ToggleGroupSingleProps | ToggleGroupMultipleProps): Node => {
  const classes = clsx(
    "flex w-fit h-fit",
    "rounded-1/2u overflow-hidden",
    "data-[orientation=vertical]:flex-col",
    props.className,
  );

  switch (props.type) {
    case "single":
      let singleProps = props as ToggleGroupSingleProps;

      return (
        <Primitive.Root
          ref={singleProps.ref}
          type={singleProps.type}
          value={singleProps.value}
          onValueChange={singleProps.onChange}
          orientation={singleProps.orientation}
          disabled={singleProps.disabled}
          style={singleProps.style}
          children={singleProps.children}
          className={classes}
        />
      );
    case "multiple":
      let multipleProps = props as ToggleGroupMultipleProps;

      return (
        <Primitive.Root
          ref={multipleProps.ref}
          type={multipleProps.type}
          value={multipleProps.value}
          onValueChange={multipleProps.onChange}
          orientation={multipleProps.orientation}
          disabled={multipleProps.disabled}
          style={multipleProps.style}
          children={multipleProps.children}
          className={classes}
        />
      );
  }
};
