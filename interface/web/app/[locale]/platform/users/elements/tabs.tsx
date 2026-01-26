// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Tabs as Primitive } from "@sptlco/design";
import { clsx } from "clsx";

/**
 * Tabs for user management.
 */
export const Tabs = {
  ...Primitive,

  /**
   * A list of user management tabs.
   */
  List: createElement<typeof Primitive.List>((props, ref) => (
    <Primitive.List {...props} ref={ref} className={clsx("flex items-center gap-2 mb-10", "w-full", props.className)} />
  )),

  /**
   * A user management tab trigger.
   */
  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "cursor-pointer relative px-4 py-2 rounded-lg",
        "inline-flex items-center h-10",
        "hover:bg-button-ghost-hover transition-all",
        "data-[state=active]:bg-background-surface"
      )}
    />
  )),

  /**
   * Content displayed on a user management tab.
   */
  Content: createElement<typeof Primitive.Content>((props, ref) => <Primitive.Content {...props} ref={ref} />)
};
