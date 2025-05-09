// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new toast title element.
 * @param props Configurable options for the element.
 * @returns A toast title element.
 */
export const ToastTitle: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Title
      ref={props.ref}
      children={props.children}
      style={props.style}
      className={clsx(
        "text-s overflow-hidden text-ellipsis whitespace-nowrap font-bold",
        props.className,
      )}
    />
  );
};
