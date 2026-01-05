// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-tooltip";
import { clsx } from "clsx";
import { cva } from "cva";

export const Tooltip = {
  /**
   * Wraps your app to provide global functionality to your tooltips.
   */
  Provider: createElement<typeof Primitive.Provider, Primitive.TooltipProviderProps>((props, ref) => <Primitive.Provider {...props} />),

  /**
   * Contains all the parts of a tooltip.
   */
  Root: createElement<typeof Primitive.Root, Primitive.TooltipProps>((props, ref) => <Primitive.Root {...props} />),

  /**
   * The button that toggles the tooltip.
   */
  Trigger: createElement<typeof Primitive.Trigger, Primitive.TooltipTriggerProps>((props, ref) => <Primitive.Trigger {...props} ref={ref} />),

  /**
   * The component that pops out when the tooltip is open.
   */
  Content: createElement<typeof Primitive.Content, Primitive.TooltipContentProps>((props, ref) => {
    const classes = cva({
      base: [
        "data-[state=delayed-open]:animate-in data-[state=delayed-open]:fade-in",
        "data-[state=closed]:animate-out data-[state=closed]:fade-out"
      ],
      variants: {
        side: {
          top: [
            "data-[state=delayed-open]:animate-in data-[state=delayed-open]:slide-in-from-bottom-5",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-bottom-5"
          ],
          bottom: [
            "data-[state=delayed-open]:animate-in data-[state=delayed-open]:slide-in-from-top-5",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-top-5"
          ],
          left: [
            "data-[state=delayed-open]:animate-in data-[state=delayed-open]:slide-in-from-right-5",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-right-5"
          ],
          right: [
            "data-[state=delayed-open]:animate-in data-[state=delayed-open]:slide-in-from-left-5",
            "data-[state=closed]:animate-out data-[state=closed]:slide-out-to-left-5"
          ]
        }
      }
    });

    return (
      <Primitive.Portal>
        <Primitive.Content {...props} ref={ref} className={clsx(classes({ side: props.side }), props.className)}>
          {props.children}
        </Primitive.Content>
      </Primitive.Portal>
    );
  })
};
