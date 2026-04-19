// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";

import { Button, Combobox, createElement, Icon, Span } from "@sptlco/design";

const fields: Record<string, { name: string; icon: string }> = {
  date: { name: "Date", icon: "calendar_today" },
  name: { name: "Name", icon: "match_case" },
  topic: { name: "Topic", icon: "label" }
};

export type SortOrder = `${"date" | "name" | "topic"}-${"asc" | "desc"}` | "";

export const Sort = createElement<typeof Fragment, { selection?: SortOrder; onSelectionChange?: (selection: SortOrder) => void }>((props, _) => {
  const selection = props.selection ?? "";

  const toggle = (field: string) => {
    const asc = `${field}-asc` as SortOrder;
    const desc = `${field}-desc` as SortOrder;

    let next: SortOrder;

    if (selection === asc) next = desc;
    else if (selection === desc) next = "";
    else next = asc;

    props.onSelectionChange?.(next);
  };

  const active = selection !== "";
  const activeField = active ? selection.replace(/-.*/, "") : null;
  const direction = selection.endsWith("asc") ? (
    <Icon symbol="sort" size={16} className="-scale-x-100 rotate-90" />
  ) : selection.endsWith("desc") ? (
    <Icon symbol="sort" size={16} className="-rotate-90" />
  ) : null;

  return (
    <Combobox.Root multiple selection={activeField ? [activeField] : []} onSelect={toggle}>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="sort" className={active ? "fill" : "group-data-[state=open]:fill"} />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List label="Sort by">
          {Object.entries(fields).map(([key, field]) => (
            <Combobox.Item
              key={key}
              value={key}
              label={field.name}
              indicator={activeField === key ? <Span className="text-hint">{direction}</Span> : undefined}
            />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
});
