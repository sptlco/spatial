// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { cva } from "cva";
import { createElement } from "..";

/**
 * Configurable options for a button.
 */
export type ButtonProps = {
  /**
   * The button's intent.
   */
  intent?: "primary" | "secondary" | "ghost" | "destructive";

  /**
   * The shape of the button.
   */
  shape?: "round" | "pill";

  /**
   * The size of the button.
   */
  size?: "small" | "medium";

  /**
   * The horizontal alignment of the button's content.
   */
  align?: "left" | "right" | "center";
};

const styles = cva({
  base: "w-fit flex items-center transition-all cursor-pointer truncate disabled:opacity-50 disabled:pointer-events-none",
  variants: {
    intent: {
      primary: "bg-button-primary hover:bg-button-primary-hover active:bg-button-primary-active",
      secondary: "bg-button-secondary hover:bg-button-secondary-hover active:bg-button-secondary-active",
      ghost: "bg-button-ghost hover:bg-button-ghost-hover active:bg-button-ghost-active",
      destructive: "bg-button-destructive hover:bg-button-destructive-hover active:bg-button-destructive-active"
    },
    shape: {
      round: "rounded-lg",
      pill: "rounded-full"
    },
    size: {
      small: "px-4 py-1.5 gap-4 text-sm",
      medium: "px-8 py-2 gap-4 text-base"
    },
    align: {
      left: "pl-2.5! justify-start",
      right: "pr-2.5! justify-end",
      center: "justify-center"
    }
  }
});

/**
 * An interactive element that prompts a user action.
 */
export const Button = createElement<"button", ButtonProps>(
  ({ intent = "primary", shape = "round", size = "medium", align = "center", type = "button", ...props }, ref) => (
    <button {...props} type={type} className={clsx(styles({ intent, shape, size, align }), props.className)} ref={ref} />
  )
);
