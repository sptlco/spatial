// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";

import { Button, createElement, Dropdown, Icon } from "@sptlco/design";

export const Filters = createElement<typeof Fragment>(() => {
  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>
        <Button intent="ghost" className="px-2!">
          <Icon symbol="filter_list" fill />
        </Button>
      </Dropdown.Trigger>
    </Dropdown.Root>
  );
});
