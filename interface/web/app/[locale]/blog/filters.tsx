// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";

import { Button, createElement, Dropdown, Icon } from "@sptlco/design";

export const Filters = createElement<typeof Fragment>(() => {
  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>
        <Button intent="ghost" size="fit" className="p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="filter_list" fill />
        </Button>
      </Dropdown.Trigger>
      <Dropdown.Content>Filters</Dropdown.Content>
    </Dropdown.Root>
  );
});
