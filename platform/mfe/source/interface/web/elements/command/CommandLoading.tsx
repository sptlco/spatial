// Copyright © Spatial. All rights reserved.

import { Command as Primitive } from "cmdk";
import { Element, ElementProps, Node, Spinner } from "..";

/**
 * Create a new command loading element.
 * @param props Configurable options for the element.
 * @returns A command loading element.
 */
export const CommandLoading: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Loading className="py-3/2u flex items-center justify-center">
      <Spinner className="size-1u" />
    </Primitive.Loading>
  );
};
