// Copyright © Spatial Corporation. All rights reserved.

import { Command as Primitive } from "cmdk";
import { CommandInputProps, Div, Element, Icon, Node } from "..";
import clsx from "clsx";

/**
 * Create a new command input element.
 * @param props Configurable options for the element.
 * @returns A command input element.
 */
export const CommandInput: Element<CommandInputProps> = ({
  placeholder = "What are you looking for?",
  ...props
}: CommandInputProps): Node => {
  return (
    <Div
      className={clsx(
        "flex w-full items-center",
        "px-1/2u py-1/4u space-x-1/2u !mt-0",
      )}
    >
      <Icon className="cursor-pointer" icon="search" />
      <Primitive.Input
        ref={props.ref}
        style={props.style}
        value={props.value}
        disabled={props.disabled}
        onValueChange={props.onChange}
        children={props.children}
        placeholder={placeholder}
        className={clsx(
          "grow",
          "transition-all",
          "!outline-none",
          "bg-transparent focus-visible:outline-none",
          "placeholder:text-foreground-quaternary",
          "disabled:opacity-50",
          props.className,
        )}
      />
    </Div>
  );
};
