// Copyright © Spatial. All rights reserved.

"use client";

import { Drawer as Primitive } from "vaul";
import { DrawerProps, Element, Node } from "..";

/**
 * Create a new drawer element.
 * @param props Configurable options for the element.
 * @returns A drawer element.
 */
export const Drawer: Element<DrawerProps> = (props: DrawerProps): Node => {
  return (
    <Primitive.Root
      open={props.open}
      onOpenChange={props.onOpenChange}
      modal={props.modal}
      shouldScaleBackground
      snapPoints={props.snapPoints}
      activeSnapPoint={props.activeSnapPoint}
      setActiveSnapPoint={props.setActiveSnapPoint}
      snapToSequentialPoint={props.snapToSequentialPoint}
      children={props.children}
    />
  );
};
