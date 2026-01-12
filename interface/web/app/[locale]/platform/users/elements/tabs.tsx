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
    <Primitive.List {...props} ref={ref} className={clsx("flex items-center gap-10 mb-10", "w-full border-b border-b-line-base")} />
  )),

  /**
   * A user management tab trigger.
   */
  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "cursor-pointer relative",
        "inline-flex items-center h-10 font-semibold",
        "data-[state=active]:text-blue data-[state=active]:after:absolute",
        "data-[state=active]:after:flex data-[state=active]:after:content-['']",
        "data-[state=active]:after:h-0.5 data-[state=active]:after:bg-blue data-[state=active]:after:w-full data-[state=active]:after:-bottom-0.25"
      )}
    />
  )),

  /**
   * Content displayed on a user management tab.
   */
  Content: createElement<typeof Primitive.Content>((props, ref) => (
    <Primitive.Content {...props} ref={ref} className={clsx("rounded-4xl bg-background-surface p-10")} />
  ))
};
