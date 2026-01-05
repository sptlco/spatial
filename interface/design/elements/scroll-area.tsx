// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-scroll-area";
import { clsx } from "clsx";
import { cva } from "cva";

/**
 * Augments native scroll functionality for custom, cross-browser styling.
 */
export const ScrollArea = {
  /**
   * Contains all the parts of a scroll area.
   */
  Root: createElement<typeof Primitive.Root, Primitive.ScrollAreaProps>((props, ref) => <Primitive.Root {...props} ref={ref} />),

  /**
   * The viewport area of the scroll area.
   */
  Viewport: createElement<typeof Primitive.Viewport, Primitive.ScrollAreaViewportProps>((props, ref) => (
    <Primitive.Viewport {...props} className="size-full" ref={ref} />
  )),

  /**
   * The vertical scrollbar.
   */
  Scrollbar: createElement<typeof Primitive.Scrollbar, Primitive.ScrollAreaScrollbarProps>(({ orientation = "vertical", ...props }, ref) => {
    const classes = cva({
      base: ["group", "flex touch-none select-none", "transition-colors", "p-px"],
      variants: {
        orientation: {
          vertical: "w-1.5",
          horizontal: "h-1.5 flex-col"
        }
      }
    });

    return <Primitive.Scrollbar {...props} ref={ref} className={clsx(classes({ orientation }), props.className)} />;
  }),

  /**
   * The thumb to be used in a scrollbar.
   */
  Thumb: createElement<typeof Primitive.Thumb, Primitive.ScrollAreaThumbProps>((props, ref) => (
    <Primitive.Thumb
      {...props}
      ref={ref}
      className={clsx(
        "transition-colors",
        "rounded-full relative flex-1",
        "bg-button-secondary group-hover:bg-button-secondary-hover",
        props.className
      )}
    />
  )),

  /**
   * The corner where both vertical and horizontal scrollbars meet.
   */
  Corner: createElement<typeof Primitive.Corner, Primitive.ScrollAreaCornerProps>((props, ref) => <Primitive.Corner {...props} ref={ref} />)
};
