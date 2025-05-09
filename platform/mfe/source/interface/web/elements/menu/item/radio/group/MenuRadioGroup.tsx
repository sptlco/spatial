// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { MenuRadioGroupProps, Element, Node } from "../../../..";
import clsx from "clsx";

/**
 * Create a new menu radio group element.
 * @param props Configurable options for the element.
 * @returns A menu radio group element.
 */
export const MenuRadioGroup: Element<MenuRadioGroupProps> = (
  props: MenuRadioGroupProps,
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
