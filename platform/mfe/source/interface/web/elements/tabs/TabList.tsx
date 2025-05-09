// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-tabs";
import { Element, ElementProps, Node, ScrollArea } from "..";
import clsx from "clsx";

/**
 * Create a new tab list element.
 * @param props Configurable options for the element.
 * @returns A tab list element.
 */
export const TabList: Element = (props: ElementProps): Node => {
  return (
    <ScrollArea>
      <Primitive.List
        ref={props.ref}
        children={props.children}
        style={props.style}
        className={clsx(
          "data-[orientation=horizontal]:min-w-max",
          "data-[orientation=horizontal]:flex-col",
          "inline-flex data-[orientation=vertical]:items-center",
          props.className,
        )}
      />
    </ScrollArea>
  );
};
