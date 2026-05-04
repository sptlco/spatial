// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon, ToggleGroup } from "@sptlco/design";

/**
 * A list of available layouts.
 */
export const views = [
  { name: "grid", icon: <Icon symbol="grid_view" fill /> },
  { name: "list", icon: <Icon symbol="format_list_bulleted" className="font-light" /> }
] as const;

/**
 * A view type.
 */
export type ViewType = (typeof views)[number]["name"];

/**
 * Allows the user to configure how assets are laid out.
 */
export const View = createElement<typeof ToggleGroup.Root>((props, ref) => {
  return (
    <ToggleGroup.Root {...props} ref={ref} className="flex items-center gap-4 mx-auto">
      {views.map((view, i) => (
        <ToggleGroup.Item key={i} value={view.name} className="flex items-center justify-center text-hint data-[state=on]:text-foreground-primary">
          {view.icon}
        </ToggleGroup.Item>
      ))}
    </ToggleGroup.Root>
  );
});
