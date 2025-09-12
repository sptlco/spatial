// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Button, SheetClose, Div, Element, ElementProps, Icon, Node } from "..";

/**
 * Create a new sheet header element.
 * @param props Configurable options for the element.
 * @returns A sheet header element.
 */
export const SheetHeader: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx("relative flex flex-col", props.className)}
    >
      <SheetClose className="!absolute right-0 top-0">
        <Button
          intent="tertiary"
          className="!py-1/4u !min-w-0 !bg-transparent !px-0"
        >
          <Icon icon="close" />
        </Button>
      </SheetClose>
      {props.children}
    </Div>
  );
};
