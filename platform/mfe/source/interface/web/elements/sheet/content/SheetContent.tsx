// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { Element, SheetContentProps, Node } from "../..";
import clsx from "clsx";
import { cva } from "class-variance-authority";

const classes = cva(
  [
    "z-50",
    "fixed p-3/2u",
    "transition ease-in-out",
    "bg-background-primary",
    "space-y-3/2u flex flex-col",
    "data-[state=open]:animate-in data-[state=closed]:animate-out",
    "data-[state=open]:fade-in data-[state=closed]:fade-out",
    "data-[state=closed]:duration-300 data-[state=open]:duration-500",
  ],
  {
    variants: {
      side: {
        right: [
          "inset-y-0 right-0",
          "h-full w-[75vw] sm:max-w-24u",
          "data-[state=open]:slide-in-from-right data-[state=closed]:slide-out-to-right",
        ],
        left: [
          "inset-y-0 left-0",
          "h-full w-[75vw] sm:max-w-24u",
          "data-[state=open]:slide-in-from-left data-[state=closed]:slide-out-to-left",
        ],
        top: [
          "inset-x-0 top-0",
          "w-full",
          "data-[state=open]:slide-in-from-top data-[state=closed]:slide-out-to-top",
        ],
        bottom: [
          "inset-x-0 bottom-0",
          "w-full",
          "data-[state=open]:slide-in-from-bottom data-[state=closed]:slide-out-to-bottom",
        ],
      },
    },
  },
);

/**
 * Create a new sheet content element.
 * @param props Configurable options for the element.
 * @returns A sheet content element.
 */
export const SheetContent: Element<SheetContentProps> = ({
  side = "right",
  ...props
}: SheetContentProps): Node => {
  return (
    <Primitive.Portal>
      <Primitive.Overlay
        className={clsx(
          "bg-background-overlay fixed inset-0 z-40",
          "data-[state=open]:animate-reveal",
          "data-[state=closed]:animate-vanish",
        )}
      />
      <Primitive.Content
        ref={props.ref}
        style={props.style}
        children={props.children}
        className={clsx(classes({ side }), props.className)}
      />
    </Primitive.Portal>
  );
};
