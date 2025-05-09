// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { DropdownRadioGroupProps, Element, Node } from "../../../..";
import clsx from "clsx";

/**
 * Create a new dropdown radio group element.
 * @param props Configurable options for the element.
 * @returns A dropdown radio group element.
 */
export const DropdownRadioGroup: Element<DropdownRadioGroupProps> = (
  props: DropdownRadioGroupProps,
): Node => {
  return (
    <Primitive.RadioGroup
      ref={props.ref}
      style={props.style}
      value={props.value}
      onValueChange={props.onChange}
      className={clsx("space-y-1/2u flex flex-col", props.className)}
      children={props.children}
    />
  );
};
