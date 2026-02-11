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
  intent?: "default" | "ghost" | "highlight" | "destructive" | "none";

  /**
   * The shape of the button.
   */
  shape?: "square" | "round" | "pill";

  /**
   * The size of the button.
   */
  size?: "small" | "medium" | "fit";

  /**
   * The horizontal alignment of the button's content.
   */
  align?: "left" | "right" | "center";

  /**
   * Whether or not the button is destructive.
   */
  destructive?: boolean;
};

const styles = cva({
  base: "w-fit flex gap-4 items-center transition-all cursor-pointer truncate disabled:opacity-50 disabled:pointer-events-none",
  variants: {
    intent: {
      default: "bg-button hover:bg-button-hover active:bg-button-active",
      destructive: "bg-button-destructive hover:bg-button-destructive-hover active:bg-button-destructive-active",
      ghost: "bg-button-ghost hover:bg-button-ghost-hover active:bg-button-ghost-active",
      highlight: "bg-button-highlight hover:bg-button-highlight-hover active:bg-button-highlight-active",
      none: ""
    },
    shape: {
      square: "rounded-none",
      round: "rounded-lg",
      pill: "rounded-full"
    },
    size: {
      small: "px-4 py-1.5 text-sm",
      medium: "px-8 py-2 text-base",
      fit: ""
    },
    align: {
      left: "pl-2.5! justify-start",
      right: "pr-2.5! justify-end",
      center: "justify-center"
    },
    destructive: {
      true: [
        "text-button-destructive!",
        "hover:bg-button-destructive-hover! hover:text-white!",
        "active:bg-button-destructive-active! active:text-white!"
      ]
    }
  }
});

/**
 * An interactive element that prompts a user action.
 */
export const Button = createElement<"button", ButtonProps>(
  ({ intent = "default", shape = "round", size = "medium", align = "center", type = "button", destructive = false, ...props }, ref) => (
    <button {...props} type={type} className={clsx(styles({ intent, shape, size, align, destructive }), props.className)} ref={ref} />
  )
);
