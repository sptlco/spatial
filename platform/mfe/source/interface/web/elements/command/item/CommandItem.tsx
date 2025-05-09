// Copyright © Spatial. All rights reserved.

import { Command as Primitive } from "cmdk";
import * as Dialog from "@radix-ui/react-dialog";
import { A, CommandItemProps, Element, Icon, Node, Span } from "../..";
import clsx from "clsx";

/**
 * Create a new command item element.
 * @param props Configurable options for the element.
 * @returns A command item element.
 */
export const CommandItem: Element<CommandItemProps> = ({
  inset = true,
  ...props
}: CommandItemProps): Node => {
  return (
    <Primitive.Item
      ref={props.ref}
      style={props.style}
      onSelect={props.onSelect}
    >
      <A href={props.href} className={clsx("flex w-full items-center")}>
        <Dialog.Close
          className={clsx(
            "transition-all",
            "rounded-1/2u",
            "cursor-pointer",
            "text-left",
            "hover:bg-background-secondary",
            "focus:bg-background-secondary",
            "active:bg-background-secondary",
            "space-x-1/2u px-1/2u py-1/4u flex size-full items-center",
            props.className,
          )}
        >
          {(inset || props.icon) && (
            <Span className="size-3/2u flex items-center justify-center">
              {props.icon && <Icon icon={props.icon} />}
            </Span>
          )}
          <Span className="grow">{props.children}</Span>
          {props.shortcut && (
            <Span className="size-3/2u text-s text-foreground-tertiary flex items-center justify-center">
              {props.shortcut}
            </Span>
          )}
        </Dialog.Close>
      </A>
    </Primitive.Item>
  );
};
