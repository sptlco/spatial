// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new toast description element.
 * @param props Configurable options for the element.
 * @returns A toast description element.
 */
export const ToastDescription: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Description
      ref={props.ref}
      children={props.children}
      style={props.style}
      className={clsx("text-s line-clamp-3", props.className)}
    />
  );
};
