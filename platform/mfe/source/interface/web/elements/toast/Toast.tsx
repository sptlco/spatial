// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-toast";
import {
  Div,
  Element,
  Node,
  ToastAction,
  ToastClose,
  ToastDescription,
  ToastIcon,
  ToastProps,
  ToastTitle,
} from "..";
import { cva } from "class-variance-authority";
import clsx from "clsx";

const classes = cva(
  [
    "group",
    "overflow-hidden",
    "pointer-events-auto relative",
    "flex w-full items-center justify-between px-1u py-1u !pr-5/2u space-x-1u",
    "rounded-1/2u",
    "transition-all",
    "data-[swipe=cancel]:translate-x-0",
    "data-[swipe=end]:translate-x-[var(--radix-toast-swipe-end-x)]",
    "data-[swipe=move]:translate-x-[var(--radix-toast-swipe-move-x)]",
    "data-[swipe=move]:transition-none",
    "data-[state=open]:animate-in data-[state=closed]:animate-out",
    "data-[state=open]:fade-in data-[state=closed]:fade-out",
    "data-[state=open]:slide-in-from-right-full",
    "data-[state=closed]:slide-out-to-right-full",
    "data-[swipe=end]:animate-out",
  ],
  {
    variants: {
      severity: {
        accent: ["bg-background-accent", "text-base-white-9"],
        information: [
          "bg-background-primary",
          "text-foreground-primary",
          "outline outline-1 outline-border-bounds",
        ],
        success: ["bg-background-severe-success text-base-white-9"],
        attention: ["bg-background-severe-attention text-base-white-9"],
        warning: ["bg-background-severe-warning text-base-white-9"],
        danger: ["bg-background-severe-danger text-base-white-9"],
        done: ["bg-background-severe-done text-base-white-9"],
      },
    },
  },
);

/**
 * Create a new toast element.
 * @param props Configurable options for the element.
 * @returns A toast element.
 */
export const Toast: Element<ToastProps> = ({
  severity = "accent",
  ...props
}: ToastProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      type={props.type}
      duration={props.duration}
      open={props.open}
      onOpenChange={props.onChange}
      style={props.style}
      className={clsx(classes({ severity }))}
    >
      {props.icon && <ToastIcon icon={props.icon} />}
      <Div className="flex min-w-0 flex-1 flex-col">
        {props.title && <ToastTitle>{props.title}</ToastTitle>}
        <ToastDescription>{props.description}</ToastDescription>
      </Div>
      {props.action && <ToastAction {...props.action} />}
      <ToastClose />
    </Primitive.Root>
  );
};
