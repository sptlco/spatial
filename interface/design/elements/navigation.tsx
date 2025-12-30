// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-navigation-menu";

/**
 * A collection of links for navigating websites.
 */
export const Navigation = {
  /**
   * Contains all the parts of a navigation menu.
   */
  Root: createElement<typeof Primitive.Root, Primitive.NavigationMenuProps>((props, ref) => <Primitive.Root {...props} ref={ref} />),

  /**
   * Signifies a submenu.
   */
  Sub: createElement<typeof Primitive.Sub, Primitive.NavigationMenuSubProps>((props, ref) => <Primitive.Sub {...props} ref={ref} />),

  /**
   * Contains the top level menu items.
   */
  List: createElement<typeof Primitive.List, Primitive.NavigationMenuListProps>((props, ref) => <Primitive.List {...props} ref={ref} />),

  /**
   * A top level menu item.
   */
  Item: createElement<typeof Primitive.Item, Primitive.NavigationMenuItemProps>((props, ref) => <Primitive.Item {...props} ref={ref} />),

  /**
   * The button that toggles the content.
   */
  Trigger: createElement<typeof Primitive.Trigger, Primitive.NavigationMenuTriggerProps>((props, ref) => <Primitive.Trigger {...props} ref={ref} />),

  /**
   * Contains the content associated with each trigger.
   */
  Content: createElement<typeof Primitive.Content, Primitive.NavigationMenuContentProps>((props, ref) => <Primitive.Content {...props} ref={ref} />),

  /**
   * A navigational link.
   */
  Link: createElement<typeof Primitive.Link, Primitive.NavigationMenuLinkProps>((props, ref) => <Primitive.Link {...props} ref={ref} />),

  /**
   * An optional indicator element that renders below the list, is used to highlight the currently active trigger.
   */
  Indicator: createElement<typeof Primitive.Indicator, Primitive.NavigationMenuIndicatorProps>((props, ref) => (
    <Primitive.Indicator {...props} ref={ref} />
  )),

  /**
   * An optional viewport element that is used to render active content outside of the list.
   */
  Viewport: createElement<typeof Primitive.Viewport, Primitive.NavigationMenuViewportProps>((props, ref) => (
    <Primitive.Viewport {...props} ref={ref} />
  ))
};
