// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-popover";
import { Element, Node, PopupProps } from "..";

/**
 * Create a new popup element.
 * @param props Configurable options for the element.
 * @returns A popup element.
 */
export const Popup: Element<PopupProps> = (props: PopupProps): Node => {
  return <Primitive.Root children={props.children} />;
};
