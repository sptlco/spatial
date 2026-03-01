// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Fragment } from "react";

import { Button, createElement, Dialog, Field, Icon } from "@sptlco/design";

/**
 * Allows the user to search the blog.
 */
export const Search = createElement<typeof Fragment>(() => {
  return (
    <Dialog.Root>
      <Dialog.Trigger asChild>
        <Button intent="ghost" className="px-2!">
          <Icon symbol="search" />
        </Button>
      </Dialog.Trigger>
      <Dialog.Content className="p-2! rounded-none! bg-transparent!" closeButton={false}>
        <Field type="text" id="search" name="search" placeholder="super asymmetry" />
      </Dialog.Content>
    </Dialog.Root>
  );
});
