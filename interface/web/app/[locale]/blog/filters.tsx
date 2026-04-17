// Copyright © Spatial Corporation. All rights reserved.

import { Fragment, useState } from "react";

import { Button, Combobox, createElement, Icon } from "@sptlco/design";

import { posts } from "./config.json";

export const Filters = createElement<typeof Fragment, { selection?: string[]; onSelectionChange?: (selection: string[]) => void }>((props, _) => {
  const [internalSelection, setInternalSelection] = useState<string[]>(props.selection ?? []);

  const controller = props.selection !== undefined;
  const selection = controller ? props.selection! : internalSelection;

  const filter = (topic: string) => {
    const next = selection.includes(topic) ? selection.filter((t) => t !== topic) : [...selection, topic];

    if (!controller) {
      setInternalSelection(next);
    }

    props.onSelectionChange?.(next);
  };

  const topics = [...new Set(posts.map((p) => p.topic).sort())];

  return (
    <Combobox.Root multiple selection={props.selection} onSelect={filter}>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="filter_alt" className="group-data-[state=open]:fill" />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List label="Topic">
          {topics.map((topic, i) => (
            <Combobox.Item key={i} value={topic} label={topic} />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
});
