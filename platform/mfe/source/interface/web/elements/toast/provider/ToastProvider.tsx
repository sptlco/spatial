// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import { Element, Node, ToastProviderProps, ToastViewport } from "../..";

/**
 * Create a new toast provider element.
 * @param props Configurable options for the element.
 * @returns A toast provider element.
 */
export const ToastProvider: Element<ToastProviderProps> = (
  props: ToastProviderProps,
): Node => {
  return (
    <Primitive.Provider
      duration={props.duration}
      label={props.label}
      swipeDirection={props.swipeDirection}
      swipeThreshold={props.swipeThreshold}
    >
      {props.children}
      <ToastViewport />
    </Primitive.Provider>
  );
};
