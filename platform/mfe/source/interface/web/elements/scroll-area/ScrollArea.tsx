// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-scroll-area";
import { Element, ElementProps, Node, Scrollbar } from "..";
import clsx from "clsx";

type ScrollAreaProps = ElementProps & {
  viewportClassName?: string;
  orientation?: "horizontal" | "vertical" | "both";
};

/**
 * Create a new scroll area element.
 * @param props Configurable options for the element.
 * @returns A scroll area element.
 */
export const ScrollArea: Element<ScrollAreaProps> = ({
  orientation = "both",
  ...props
}: ScrollAreaProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      style={props.style}
      className={clsx("relative overflow-hidden", props.className)}
    >
      <Primitive.Viewport
        children={props.children}
        className={clsx("size-full", props.viewportClassName)}
      />
      {(orientation == "vertical" || orientation == "both") && (
        <Scrollbar orientation="vertical" />
      )}
      {(orientation == "horizontal" || orientation == "both") && (
        <Scrollbar orientation="horizontal" />
      )}
      <Primitive.Corner />
    </Primitive.Root>
  );
};
