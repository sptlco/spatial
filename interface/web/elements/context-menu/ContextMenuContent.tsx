// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new context menu content element.
 * @param props Configurable options for the element.
 * @returns A context menu content element.
 */
export const ContextMenuContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Content
        ref={props.ref}
        style={props.style}
        children={props.children}
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
