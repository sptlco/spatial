// Copyright © Spatial. All rights reserved.

import { cva } from "class-variance-authority";
import { clsx } from "clsx";
import { ButtonLabel, ButtonProps, Element, Icon, Node, Spinner } from "..";
import { Button as Primitive } from "@headlessui/react";

const classes = cva(
  [
    "transition-all ",
    "w-fit h-fit max-h-5/2u px-3/2u space-x-1/2u",
    "flex relative items-center justify-center shrink-0 whitespace-nowrap",
    "disabled:opacity-50 disabled:pointer-events-none",
  ],
  {
    variants: {
      size: {
        large: ["min-h-2u py-1/2u min-w-4u"],
        small: ["text-s", "min-h-2u py-1/4u min-w-3u"],
      },
      roundness: {
        round: "rounded-1/2u",
        pill: "rounded-full",
      },
      intent: {
        primary: [
          "bg-background-interactive-primary-default text-foreground-inverted-primary",
          "hover:enabled:bg-background-interactive-primary-hover",
          "focus:enabled:bg-background-interactive-primary-focus",
          "active:enabled:bg-background-interactive-primary-active",
        ],
        secondary: [
          "bg-background-interactive-secondary-default text-foreground-primary",
          "hover:enabled:bg-background-interactive-secondary-hover",
          "focus:enabled:bg-background-interactive-secondary-focus",
          "active:enabled:bg-background-interactive-secondary-active",
        ],
        tertiary: [
          "bg-background-interactive-tertiary-default text-foreground-primary",
          "hover:enabled:bg-background-interactive-tertiary-hover",
          "focus:enabled:bg-background-interactive-tertiary-focus",
          "active:enabled:bg-background-interactive-tertiary-active",
        ],
        outline: [
          "outline outline-1",
          "bg-background-interactive-outline-default text-foreground-primary outline-border-interactive-outline-default",
          "hover:enabled:bg-background-interactive-outline-hover hover:enabled:text-foreground-inverted-primary hover:enabled:outline-border-interactive-outline-hover",
          "focus:enabled:bg-background-interactive-outline-focus focus:enabled:text-foreground-inverted-primary focus:enabled:outline-border-interactive-outline-focus",
          "active:enabled:bg-background-interactive-outline-active active:enabled:text-foreground-inverted-primary active:enabled:outline-border-interactive-outline-active",
        ],
        accent: [
          "bg-background-interactive-accent-default text-base-white-9",
          "hover:enabled:bg-background-interactive-accent-hover",
          "focus:enabled:bg-background-interactive-accent-focus",
          "active:enabled:bg-background-interactive-accent-active",
        ],
        information: [
          "bg-background-interactive-information-default text-foreground-inverted-primary",
          "hover:enabled:bg-background-interactive-information-hover",
          "focus:enabled:bg-background-interactive-information-focus",
          "active:enabled:bg-background-interactive-information-active",
        ],
        success: [
          "bg-background-interactive-success-default text-base-white-9",
          "hover:enabled:bg-background-interactive-success-hover",
          "focus:enabled:bg-background-interactive-success-focus",
          "active:enabled:bg-background-interactive-success-active",
        ],
        attention: [
          "bg-background-interactive-attention-default text-base-white-9",
          "hover:enabled:bg-background-interactive-attention-hover",
          "focus:enabled:bg-background-interactive-attention-focus",
          "active:enabled:bg-background-interactive-attention-active",
        ],
        warning: [
          "bg-background-interactive-warning-default text-base-white-9",
          "hover:enabled:bg-background-interactive-warning-hover",
          "focus:enabled:bg-background-interactive-warning-focus",
          "active:enabled:bg-background-interactive-warning-active",
        ],
        danger: [
          "bg-background-interactive-danger-default text-base-white-9",
          "hover:enabled:bg-background-interactive-danger-hover",
          "focus:enabled:bg-background-interactive-danger-focus",
          "active:enabled:bg-background-interactive-danger-active",
        ],
        done: [
          "bg-background-interactive-done-default text-base-white-9",
          "hover:enabled:bg-background-interactive-done-hover",
          "focus:enabled:bg-background-interactive-done-focus",
          "active:enabled:bg-background-interactive-done-active",
        ],
      },
    },
  },
);

/**
 * Create a new button element.
 * @param props Configurable options for the element.
 * @returns A button element.
 */
export const Button: Element<ButtonProps> = ({
  size = "large",
  intent = "primary",
  roundness = "round",
  ...props
}: ButtonProps): Node => {
  return (
    <Primitive
      ref={props.ref}
      style={props.style}
      type={props.type}
      onClick={props.onClick}
      disabled={props.disabled}
      className={clsx(
        "group",
        classes({ intent, size, roundness }),
        { "pointer-events-none": props.loading },
        props.className,
      )}
    >
      {props.direction == "backward" && (
        <Icon
          icon="arrow_left_alt"
          className={clsx("group-hover:-translate-x-1/4u transition-all", {
            "opacity-0": props.loading,
            "!text-l": size == "small",
          })}
        />
      )}
      {props.children && (
        <ButtonLabel
          className={clsx("transition-all", {
            "opacity-0": props.loading,
          })}
          children={props.children}
        />
      )}
      {props.direction == "forward" && (
        <Icon
          icon="arrow_right_alt"
          className={clsx("group-hover:translate-x-1/4u transition-all", {
            "opacity-0": props.loading,
            "!text-l": size == "small",
          })}
        />
      )}
      {props.loading && <Spinner className={clsx("size-3/2u absolute")} />}
    </Primitive>
  );
};
