// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-select";
import { Element, ElementProps, Node, SelectGroupProps } from "../../..";
import clsx from "clsx";

/**
 * Create a new select group element.
 * @param props Configurable options for the element.
 * @returns A select group element.
 */
export const SelectGroup: Element<SelectGroupProps> = (
  props: SelectGroupProps,
): Node => {
  return (
    <Primitive.Group
      ref={props.ref}
      style={props.style}
      className={clsx("space-y-1/2u flex flex-col", props.className)}
    >
      {props.heading && (
        <Primitive.Label
          children={props.heading}
          className={clsx(
            "px-1/2u py-1/4u flex w-full",
            "text-s text-foreground-tertiary font-bold",
          )}
        />
      )}
      {props.children}
    </Primitive.Group>
  );
};
