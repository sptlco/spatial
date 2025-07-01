// Copyright © Spatial Corporation. All rights reserved.

import { Command as Primitive } from "cmdk";
import { CommandSeparatorProps, Element, Node } from "../..";
import { cva } from "class-variance-authority";

const classes = cva(["flex bg-border-divider"], {
  variants: {
    orientation: {
      horizontal: ["w-full h-[1px] my-1/2u"],
      vertical: ["h-full w-[1px] mx-1/2u"],
    },
  },
});

/**
 * Create a new command separator element.
 * @param props Configurable options for the element.
 * @returns A command separator element.
 */
export const CommandSeparator: Element<CommandSeparatorProps> = ({
  orientation = "horizontal",
  ...props
}: CommandSeparatorProps): Node => {
  return (
    <Primitive.Separator
      ref={props.ref}
      style={props.style}
      className={classes({ orientation })}
      children={props.children}
    />
  );
};
