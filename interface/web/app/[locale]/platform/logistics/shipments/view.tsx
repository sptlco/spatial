// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon, ToggleGroup } from "@sptlco/design";

/**
 * A list of available layouts.
 */
export const VIEWS = [
  { name: "grid", icon: <Icon symbol="grid_view" fill /> },
  { name: "list", icon: <Icon symbol="format_list_bulleted" className="font-light" /> }
];

/**
 * Allows the user to configure how posts are laid out.
 */
export const View = createElement<typeof ToggleGroup.Root>((props, ref) => {
  return (
    <ToggleGroup.Root {...props} ref={ref} className="flex items-center gap-4">
      {VIEWS.map((view, i) => (
        <ToggleGroup.Item
          key={i}
          value={view.name}
          className="flex items-center justify-center text-background-highlight data-[state=on]:text-foreground-primary"
        >
          {view.icon}
        </ToggleGroup.Item>
      ))}
    </ToggleGroup.Root>
  );
});
