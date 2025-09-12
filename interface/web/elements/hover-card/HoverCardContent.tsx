// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-hover-card";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new hover card content element.
 * @param props Configurable options for the element.
 * @returns A hover card content element.
 */
export const HoverCardContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Content
        ref={props.ref}
        style={props.style}
        sideOffset={8}
        collisionPadding={8}
        children={props.children}
        className={clsx(
          "rounded-1/2u p-3/2u",
          "bg-background-primary",
          "space-y-3/2u flex flex-col",
          "outline-border-bounds outline outline-1",
          "data-[state=open]:animate-grow",
          "data-[state=closed]:animate-shrink",
        )}
      />
    </Primitive.Portal>
  );
};
