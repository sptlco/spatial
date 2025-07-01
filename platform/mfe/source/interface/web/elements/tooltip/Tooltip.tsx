// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-tooltip";
import { Element, Node, TooltipProps } from "..";

/**
 * Create a new tooltip element.
 * @param props Configurable options for the element.
 * @returns A tooltip element.
 */
export const Tooltip: Element<TooltipProps> = (props: TooltipProps): Node => {
  return (
    <Primitive.Root
      open={props.open}
      onOpenChange={props.onChange}
      children={props.children}
    />
  );
};
