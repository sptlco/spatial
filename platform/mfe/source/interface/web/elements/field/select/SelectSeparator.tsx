// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-select";
import { Element, ElementProps, Node, Separator } from "../..";

/**
 * Create a new select separator element.
 * @param props Configurable options for the element.
 * @returns A select separator element.
 */
export const SelectSeparator: Element = (props: ElementProps): Node => {
  return (
    <Primitive.Separator
      ref={props.ref}
      style={props.style}
      className={props.className}
    >
      <Separator />
    </Primitive.Separator>
  );
};
