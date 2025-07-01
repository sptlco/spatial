// Copyright © Spatial Corporation. All rights reserved.

import { Command as Primitive, useCommandState } from "cmdk";
import { Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new command list element.
 * @param props Configurable options for the element.
 * @returns A command list element.
 */
export const CommandList: Element = (props: ElementProps): Node => {
  const search = useCommandState((state) => state.search);

  return (
    <Primitive.List
      ref={props.ref}
      style={props.style}
      className={clsx("transition-all", props.className)}
    >
      <Primitive.Empty className="text-s text-foreground-secondary py-1u text-center">
        No results found for "{search}".
      </Primitive.Empty>
      {props.children}
    </Primitive.List>
  );
};
