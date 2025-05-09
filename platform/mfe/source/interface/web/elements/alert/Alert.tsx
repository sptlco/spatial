// Copyright © Spatial. All rights reserved.

import { cva } from "class-variance-authority";
import { clsx } from "clsx";
import { AlertProps, Div, Element, Node } from "..";

const classes = cva(
  [
    "rounded-1/2u",
    "flex flex-col items-center space-y-3/2u px-3/2u pt-3/2u pb-2u w-full h-fit text-center",
    "sm:flex-row sm:space-y-0 sm:space-x-3/2u sm:!py-1u sm:text-left",
  ],
  {
    variants: {
      severity: {
        accent: ["bg-background-accent text-base-white-9"],
        information: [
          "outline outline-1 outline-border-severe-information text-foreground-severe-information",
        ],
        success: [
          "outline outline-1 outline-border-severe-success text-foreground-severe-success",
        ],
        attention: [
          "outline outline-1 outline-border-severe-attention text-foreground-severe-attention",
        ],
        warning: [
          "outline outline-1 outline-border-severe-warning text-foreground-severe-warning",
        ],
        danger: [
          "outline outline-1 outline-border-severe-danger text-foreground-severe-danger",
        ],
        done: [
          "outline outline-1 outline-border-severe-done text-foreground-severe-done",
        ],
      },
    },
  },
);

/**
 * Create a new alert element.
 * @param props Configurable options for the element.
 * @returns An alert element.
 */
export const Alert: Element<AlertProps> = ({
  severity = "accent",
  ...props
}: AlertProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx(classes({ severity }), props.className)}
      children={props.children}
    />
  );
};
