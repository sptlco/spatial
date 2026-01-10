// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-tabs";

/**
 * A set of layered sections of content that are displayed one at a time.
 */
export const Tabs = {
  ...Primitive,

  /**
   * Contains all the tabs component parts.
   */
  Root: createElement<typeof Primitive.Root, Primitive.TabsProps>((props, ref) => <Primitive.Root {...props} ref={ref} />),

  /**
   * Contains the triggers that are aligned along the edge of the active content.
   */
  List: createElement<typeof Primitive.List, Primitive.TabsListProps>((props, ref) => <Primitive.List {...props} ref={ref} />),

  /**
   * The button that activated the associated content.
   */
  Trigger: createElement<typeof Primitive.Trigger, Primitive.TabsTriggerProps>((props, ref) => <Primitive.Trigger {...props} ref={ref} />),

  /**
   * Contains the content associated with each trigger.
   */
  Content: createElement<typeof Primitive.Content, Primitive.TabsContentProps>((props, ref) => <Primitive.Content {...props} ref={ref} />)
};
