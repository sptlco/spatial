// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dropdown-menu";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new dropdown content element.
 * @param props Configurable options for the element.
 * @returns A dropdown content element.
 */
export const DropdownContent: Element<DropdownContentProps> = (
  props: DropdownContentProps,
): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Content
        ref={props.ref}
        style={{
          ...props.style,
          outline: "1px solid var(--color-border-bounds",
        }}
        children={props.children}
        align={props.align}
        side={props.side}
        sideOffset={8}
        collisionPadding={8}
        className={clsx(
          "z-20",
          "group/dropdown-content",
          "min-w-12u p-1/2u space-y-1/2u flex flex-col",
          "rounded-1/2u bg-background-primary",
          "text-foreground-primary",
          "transition-all",
          "data-[state=open]:animate-in data-[state=open]:fade-in",
          "data-[state=closed]:animate-out data-[state=closed]:fade-out",
          props.className,
        )}
      />
    </Primitive.Portal>
  );
};

type DropdownContentProps = ElementProps & {
  side?: "left" | "right" | "bottom" | "top";
  align?: "center" | "end" | "start";
};
