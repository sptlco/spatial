// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-tooltip";
import { Element, Node, TooltipProviderProps } from "../..";

/**
 * Create a new tooltip provider element.
 * @param props Configurable options for the element.
 * @returns A tooltip provider element.
 */
export const TooltipProvider: Element<TooltipProviderProps> = (
  props: TooltipProviderProps,
): Node => {
  return (
    <Primitive.Provider
      delayDuration={props.delayDuration}
      skipDelayDuration={props.skipDelayDuration}
      disableHoverableContent={props.disableHoverableContent}
      children={props.children}
    />
  );
};
