// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, Node, ToastViewportProps } from "../..";
import clsx from "clsx";

/**
 * Create a new toast viewport element.
 * @param props Configurable options for the element.
 * @returns A toast viewport element.
 */
export const ToastViewport: Element<ToastViewportProps> = (
  props: ToastViewportProps,
): Node => {
  return (
    <Primitive.Viewport
      ref={props.ref}
      style={props.style}
      label={props.label}
      children={props.children}
      className={clsx(
        "fixed top-0 z-[100]",
        "flex max-h-screen w-full",
        "flex-col-reverse",
        "p-1/2u",
        "sm:bottom-0 sm:right-0 sm:top-auto sm:flex-col",
        "md:max-w-[420px]",
        props.className,
      )}
    />
  );
};
