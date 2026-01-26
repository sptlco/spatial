// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { createElement, Container, Dropdown, Field, Icon } from "..";

/**
 * Autocomplete input and command palette with a list of suggestions.
 */
export const Combobox = {
  Root: createElement<typeof Dropdown.Root, { value?: string[]; onChange?: (value: string[]) => void }>((props, ref) => {
    // ...

    return <Dropdown.Root {...props} ref={ref} />;
  }),

  Trigger: createElement<typeof Dropdown.Trigger>((props, ref) => <Dropdown.Trigger {...props} ref={ref} />),

  Content: createElement<typeof Dropdown.Portal>((props, ref) => {
    const [search, setSearch] = useState("");

    return (
      <Dropdown.Portal {...props} ref={ref}>
        <Dropdown.Content>
          <Container className="relative w-full max-w-sm flex items-center">
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search users"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-12"
            />
            <Icon symbol="search" className="absolute left-3" />
          </Container>
        </Dropdown.Content>
      </Dropdown.Portal>
    );
  }),

  Item: createElement<typeof Dropdown.CheckboxItem, { value: string }>((props, ref) => <Dropdown.CheckboxItem {...props} ref={ref} checked />)
};
