// Copyright © Spatial Corporation. All rights reserved.

import { Command as Primitive } from "cmdk";
import { CommandGroupProps, Element, Node, Span } from "../..";
import clsx from "clsx";

/**
 * Create a new command group element.
 * @param props Configurable options for the element.
 * @returns A command group element.
 */
export const CommandGroup: Element<CommandGroupProps> = (
  props: CommandGroupProps,
): Node => {
  return (
    <Primitive.Group
      ref={props.ref}
      style={props.style}
      className={clsx("space-y-1/2u flex w-full flex-col", props.className)}
      children={props.children}
      heading={
        props.heading && (
          <Span
            className={clsx(
              "px-1/2u py-1/4u flex w-full",
              "text-s text-foreground-tertiary font-bold",
            )}
          >
            {props.heading}
          </Span>
        )
      }
    />
  );
};
