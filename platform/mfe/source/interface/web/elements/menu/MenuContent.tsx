// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-menubar";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new menu content element.
 * @param props Configurable options for the element.
 * @returns A menu content element.
 */
export const MenuContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Content
        ref={props.ref}
        style={props.style}
        children={props.children}
        sideOffset={8}
        collisionPadding={8}
        className={clsx(
          "min-w-16u p-1/2u space-y-1/2u flex flex-col",
          "rounded-1/2u bg-background-primary",
          "!outline-border-bounds !outline !outline-1",
          "data-[state=open]:animate-grow",
          "data-[state=closed]:animate-shrink",
          props.className,
        )}
      />
    </Primitive.Portal>
  );
};
