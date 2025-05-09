// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-dialog";
import { DialogProps, Element, Node } from "..";

/**
 * Create a new sheet element.
 * @param props Configurable options for the element.
 * @returns A sheet element.
 */
export const Sheet: Element<DialogProps> = (props: DialogProps): Node => {
  return (
    <Primitive.Root
      open={props.open}
      onOpenChange={props.onOpenChange}
      children={props.children}
    />
  );
};
