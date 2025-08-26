// Copyright © Spatial Corporation. All rights reserved.

import * as VisuallyHidden from "@radix-ui/react-visually-hidden";
import { Command as Primitive } from "cmdk";
import {
  CommandProps,
  Dialog,
  DialogContent,
  DialogDescription,
  DialogTitle,
  Element,
  Node,
} from "..";
import clsx from "clsx";

/**
 * Create a new command element.
 * @param props Configurable options for the element.
 * @returns A command element.
 */
export const Command: Element<CommandProps> = (props: CommandProps): Node => {
  return (
    <Dialog open={props.open} onOpenChange={props.onOpenChange}>
      <DialogContent className="!space-y-0 !bg-transparent !p-0">
        <VisuallyHidden.Root asChild>
          <DialogTitle>Command</DialogTitle>
        </VisuallyHidden.Root>
        <VisuallyHidden.Root asChild>
          <DialogDescription>A command menu.</DialogDescription>
        </VisuallyHidden.Root>
        <Primitive
          ref={props.ref}
          style={props.style}
          children={props.children}
          onKeyDown={props.onKeyDown}
          className={clsx(
            "transition-all",
            "flex w-full flex-col",
            "p-1/2u space-y-1/2u",
            "rounded-1/2u bg-background-primary",
            props.className,
          )}
        />
      </DialogContent>
    </Dialog>
  );
};
