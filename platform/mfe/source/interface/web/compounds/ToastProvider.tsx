// Copyright © Spatial. All rights reserved.

"use client";

import { useToast } from "@spatial/hooks";

import {
  Element,
  Node,
  Toast,
  ToastProvider as Primitive,
} from "@spatial/elements";

/**
 * Create a new toast provider element.
 * @returns A toast provider element.
 */
export const ToastProvider: Element = (): Node => {
  const { toasts } = useToast();

  return (
    <Primitive>
      {toasts.map(({ id, ...props }) => (
        <Toast key={id} {...props} />
      ))}
    </Primitive>
  );
};
