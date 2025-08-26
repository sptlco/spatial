// Copyright © Spatial Corporation. All rights reserved.

import { cva } from "class-variance-authority";
import { Element, HR, Node, SeparatorProps } from "..";

const classes = cva(["flex border-none bg-border-divider"], {
  variants: {
    orientation: {
      horizontal: ["w-full h-[1px]"],
      vertical: ["h-full w-[1px]"],
    },
  },
});

/**
 * Create a new separator element.
 * @param props Configurable options for the element.
 * @returns A separator element.
 */
export const Separator: Element<SeparatorProps> = ({
  orientation = "horizontal",
  ...props
}: SeparatorProps): Node => {
  return (
    <HR
      ref={props.ref}
      style={props.style}
      children={props.children}
      className={classes({ orientation })}
    />
  );
};
