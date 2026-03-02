// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Fragment } from "react";

import { Button, createElement, Icon } from "@sptlco/design";

/**
 * Allows the user to search the blog.
 */
export const Search = createElement<typeof Fragment>(() => {
  return (
    <Button intent="none" size="fit" className="p-2">
      <Icon symbol="search" />
    </Button>
  );
});
