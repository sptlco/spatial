// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";

import { Button, Combobox, createElement, Icon } from "@sptlco/design";

export const Sort = createElement<typeof Fragment>(() => {
  return (
    <Combobox.Root>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="sort" fill />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>Sort</Combobox.Content>
    </Combobox.Root>
  );
});
