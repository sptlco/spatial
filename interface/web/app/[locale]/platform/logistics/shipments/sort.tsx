// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";

import { Button, createElement, Dropdown, Icon } from "@sptlco/design";

export const Sort = createElement<typeof Fragment>(() => {
  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="sort" className="group-data-[state=open]:fill" />
        </Button>
      </Dropdown.Trigger>
      <Dropdown.Content>Sort</Dropdown.Content>
    </Dropdown.Root>
  );
});
