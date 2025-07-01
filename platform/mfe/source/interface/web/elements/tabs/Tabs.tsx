// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-tabs";
import { Element, Node, TabsProps } from "..";
import clsx from "clsx";

/**
 * Create a new tab element.
 * @param props Configurable options for the element.
 * @returns A tab element.
 */
export const Tabs: Element<TabsProps> = ({
  orientation = "vertical",
  ...props
}: TabsProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      value={props.value}
      onValueChange={props.onChange}
      orientation={orientation}
      style={props.style}
      children={props.children}
      className={clsx(
        "flex",
        "data-[orientation=vertical]:flex-col",
        props.className,
      )}
    />
  );
};
