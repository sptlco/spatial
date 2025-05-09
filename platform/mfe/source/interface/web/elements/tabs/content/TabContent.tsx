// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-tabs";
import { Element, Node, TabContentProps } from "../..";
import clsx from "clsx";

/**
 * Create a new tab content element.
 * @param props Configurable options for the element.
 * @returns A tab content element.
 */
export const TabContent: Element<TabContentProps> = (
  props: TabContentProps,
): Node => {
  return (
    <Primitive.Content
      ref={props.ref}
      children={props.children}
      style={props.style}
      value={props.value}
      className={clsx(props.className)}
    />
  );
};
