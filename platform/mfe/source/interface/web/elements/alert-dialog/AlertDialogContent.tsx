// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-alert-dialog";
import { Element, ElementProps, Node } from "@spatial/elements";
import clsx from "clsx";

/**
 * Create a new alert dialog content element.
 * @param props Configurable options for the element.
 * @returns An alert dialog content element.
 */
export const AlertDialogContent: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Overlay
        className={clsx(
          "bg-background-overlay fixed inset-0",
          "data-[state=open]:animate-reveal",
          "data-[state=closed]:animate-vanish",
        )}
      />
      <Primitive.Content
        ref={props.ref}
        style={props.style}
        children={props.children}
        className={clsx(
          "rounded-1/2u bg-background-primary",
          "max-w-32u p-3/2u max-h-[85vh] w-[90vw]",
          "space-y-2u flex flex-col",
          "data-[state=open]:animate-dialog-in",
          "data-[state=closed]:animate-dialog-out",
          "fixed left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2",
          props.className,
        )}
      />
    </Primitive.Portal>
  );
};
