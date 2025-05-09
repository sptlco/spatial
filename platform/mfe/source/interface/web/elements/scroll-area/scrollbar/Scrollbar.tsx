// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-scroll-area";
import { Element, Node, ScrollbarProps } from "../..";
import clsx from "clsx";

/**
 * Create a new scrollbar element.
 * @param props Configurable options for the element.
 * @returns A scrollbar.
 */
export const Scrollbar: Element<ScrollbarProps> = ({
  orientation = "vertical",
  ...props
}: ScrollbarProps): Node => {
  return (
    <Primitive.Scrollbar
      ref={props.ref}
      style={props.style}
      orientation={orientation}
      className={clsx(
        "group",
        "p-[1px]",
        "transition-opacity",
        "flex touch-none select-none",
        "data-[orientation=horizontal]:flex-col",
        "data-[state=visible]:animate-in data-[state=hidden]:animate-out",
        "data-[state=visible]:fade-in data-[state=hidden]:fade-out",
        "data-[state=hidden]:duration-300 data-[state=visible]:duration-300",
        "data-[orientation=vertical]:w-1/4u data-[orientation=vertical]:h-full data-[orientation=vertical]:hover:!w-[6px]",
        "data-[orientation=horizontal]:h-1/4u data-[orientation=horizontal]:hover:!h-[6px]",
        props.className,
      )}
    >
      <Primitive.Thumb
        className={clsx(
          "flex-1",
          "relative",
          "rounded-full",
          "bg-border-bounds",
        )}
      />
    </Primitive.Scrollbar>
  );
};
