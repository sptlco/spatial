// Copyright © Spatial Corporation. All rights reserved.

import { AssetView } from "@sptlco/data";
import { Fragment, useState } from "react";

import { Button, Combobox, createElement, Icon } from "@sptlco/design";

export const Filters = createElement<
  typeof Fragment,
  { assets: AssetView[]; selection?: string[]; onSelectionChange?: (selection: string[]) => void }
>((props, _) => {
  const [internalSelection, setInternalSelection] = useState<string[]>(props.selection ?? []);

  const controller = props.selection !== undefined;
  const selection = controller ? props.selection! : internalSelection;

  const filter = (type: string) => {
    const next = selection.includes(type) ? selection.filter((t) => t !== type) : [...selection, type];

    if (!controller) {
      setInternalSelection(next);
    }

    props.onSelectionChange?.(next);
  };

  const types = [...new Set(props.assets.map((v) => v.asset.type).sort())];

  return (
    <Combobox.Root multiple selection={selection} onSelect={filter}>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="filter_alt" className="group-data-[state=open]:fill" />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List label="Type">
          {types.map((type, i) => (
            <Combobox.Item key={i} value={type} label={type} />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
});
