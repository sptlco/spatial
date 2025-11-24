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
   * The size of the button.
   */
  size?: "small" | "medium";
};

const styles = cva({
  base: "flex items-center rounded-full transition-all cursor-pointer",
  variants: {
    intent: {
      primary: "bg-button-primary hover:bg-button-primary-hover active:bg-button-primary-active",
      secondary: "bg-button-secondary hover:bg-button-secondary-hover active:bg-button-secondary-active",
      ghost: "bg-button-ghost hover:bg-button-ghost-hover active:bg-button-ghost-active",
      destructive: "bg-button-destructive hover:bg-button-destructive-hover active:bg-button-destructive-active"
    },
    size: {
      small: "",
      medium: "px-8 py-2 text-base"
    }
  }
});

/**
 * An interactive element that prompts a user action.
 */
export const Button = createElement<"button", ButtonProps>(({ intent = "primary", size = "medium", ...props }, ref) => (
  <button {...props} className={clsx(styles({ intent: intent, size: size }), props.className)} ref={ref} />
));
