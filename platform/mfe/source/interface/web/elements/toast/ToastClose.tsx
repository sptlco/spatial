// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, ElementProps, Icon, Node } from "..";
import clsx from "clsx";

/**
 * Create a new toast close element.
 * @param props Configurable options for the element.
 * @returns A toast close element.
 */
export const ToastClose: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Close
      ref={props.ref}
      style={props.style}
      toast-close=""
      className={clsx(
        "top-1/4u right-3/4u absolute",
        "rounded-1/2u",
        "transition-all",
        "opacity-0 focus:opacity-100",
        "!outline-none",
        "group-hover:opacity-100",
        props.className,
      )}
    >
      <Icon className="!text-l" icon="close" />
    </Primitive.Close>
  );
};
