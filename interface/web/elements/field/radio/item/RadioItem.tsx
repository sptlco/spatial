// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-radio-group";
import { Element, Node, RadioItemProps } from "../../..";
import clsx from "clsx";

/**
 * Create a new radio item element.
 * @param props Configurable options for the element.
 * @returns A radio item element.
 */
export const RadioItem: Element<RadioItemProps> = (
  props: RadioItemProps,
): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      style={props.style}
      value={props.value}
      className={clsx("group", props.className)}
    >
      {props.children}
    </Primitive.Item>
  );
};
