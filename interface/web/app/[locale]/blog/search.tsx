// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Fragment, useState } from "react";

import { Button, Command, createElement, Icon } from "@sptlco/design";

/**
 * Allows the user to search the blog.
 */
export const Search = createElement<typeof Fragment>(() => {
  const [open, setOpen] = useState(false);

  return (
    <>
      <Button intent="ghost" className="px-2!" onClick={() => setOpen(true)}>
        <Icon symbol="search" />
      </Button>
      <Command.Dialog open={open} onOpenChange={setOpen} label="Search"></Command.Dialog>
    </>
  );
});
