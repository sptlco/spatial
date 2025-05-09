// Copyright © Spatial. All rights reserved.

import { cva } from "class-variance-authority";
import { clsx } from "clsx";
import { BadgeLabel, BadgeProps, Div, Element, Node } from "..";

const classes = cva(
  [
    "h-fit w-fit px-1u py-1/4u space-x-1/2u flex items-center justify-center",
    "text-xs font-bold uppercase",
  ],
  {
    variants: {
      roundness: {
        round: ["rounded-1/4u"],
        pill: ["rounded-full"],
      },
      intent: {
        primary: [
          "bg-background-interactive-primary-default",
          "text-foreground-inverted-primary",
        ],
        secondary: [
          "bg-background-interactive-secondary-default",
          "text-foreground-primary",
        ],
        tertiary: ["!px-0 !py-0", "text-foreground-primary"],
        outline: [
          "outline outline-1 outline-border-interactive-outline-default",
          "text-foreground-primary",
        ],
        accent: [
          "bg-background-interactive-accent-default",
          "text-base-white-9",
        ],
        information: [
          "bg-background-interactive-information-default",
          "text-foreground-inverted-primary",
        ],
        success: [
          "bg-background-interactive-success-default",
          "text-base-white-9",
        ],
        attention: [
          "bg-background-interactive-attention-default",
          "text-base-white-9",
        ],
        warning: [
          "bg-background-interactive-warning-default",
          "text-base-white-9",
        ],
        danger: [
          "bg-background-interactive-danger-default",
          "text-base-white-9",
        ],
        done: ["bg-background-interactive-done-default", "text-base-white-9"],
      },
    },
  },
);

/**
 * Create a new badge element.
 * @param props Configurable options for the element.
 * @returns A badge element.
 */
export const Badge: Element<BadgeProps> = ({
  intent = "primary",
  roundness = "pill",
  ...props
}: BadgeProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx(classes({ intent, roundness }), props.className)}
    >
      <BadgeLabel children={props.children} />
    </Div>
  );
};
