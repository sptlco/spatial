// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Drawer as Primitive } from "vaul";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new drawer content element.
 * @param props Configurable options for the element.
 * @returns A drawer content element.
 */
export const DrawerContent: Element = (props: ElementProps): Node => {
  return (
    <>
      <Primitive.Overlay className="bg-background-overlay fixed inset-0 z-50 !m-0" />
      <Primitive.Portal>
        <Primitive.Content
          ref={props.ref}
          style={props.style}
          className={clsx(
            "p-2u !pt-3/2u",
            "bg-background-primary",
            "rounded-t-3/2u",
            "space-y-3/2u flex flex-col",
            "fixed bottom-0 left-0 right-0 h-full max-h-[97%]",
            props.className,
          )}
        >
          <Primitive.Handle className="!h-1/4u !w-4u !bg-border-bounds" />
          {props.children}
        </Primitive.Content>
      </Primitive.Portal>
    </>
  );
};
