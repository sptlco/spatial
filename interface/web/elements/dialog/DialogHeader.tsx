// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import {
  Button,
  DialogClose,
  Div,
  Element,
  ElementProps,
  Icon,
  Node,
} from "..";

/**
 * Create a new dialog header element.
 * @param props Configurable options for the element.
 * @returns A dialog header element.
 */
export const DialogHeader: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("relative flex flex-col", props.className)}
    >
      <DialogClose className="absolute right-0 top-0">
        <Button
          intent="tertiary"
          className="!py-1/4u !absolute !min-w-0 !bg-transparent !px-0"
        >
          <Icon icon="close" />
        </Button>
      </DialogClose>
      {props.children}
    </Div>
  );
};
