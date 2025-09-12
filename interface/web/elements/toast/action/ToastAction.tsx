// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, Node, ToastActionProps } from "../..";
import clsx from "clsx";

/**
 * Create a new toast action element.
 * @param props Configurable options for the element.
 * @returns A toast action element.
 */
export const ToastAction: Element<ToastActionProps> = (
  props: ToastActionProps,
): Node => {
  return (
    <Primitive.Action
      ref={props.ref}
      asChild={props.asChild}
      altText={props.altText}
      style={props.style}
      children={props.children}
      onClick={props.onClick}
      className={clsx(
        "rounded-1/2u text-s",
        "flex shrink-0 items-center justify-center whitespace-nowrap",
        "min-w-3u min-h-2u px-3/2u py-1/4u h-fit w-fit",
        "outline-base-white-9 outline outline-1",
        props.className,
      )}
    />
  );
};
