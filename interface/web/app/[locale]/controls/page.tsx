// Copyright © Spatial Corporation. All rights reserved.

import { Card, Dropdown, Icon } from "@sptlco/design";

/**
 * An interface for administrative operations.
 * @returns The admin interface.
 */
export default function Page() {
  return (
    <Card.Root>
      <Card.Header>
        <Card.Title className="text-3xl font-bold">Members</Card.Title>
        <Card.Description>This is a description.</Card.Description>
        <Card.Gutter>
          <Dropdown.Root>
            <Dropdown.Trigger>
              <Icon symbol="more_horiz" size={32} />
            </Dropdown.Trigger>
            <Dropdown.Content>Here's my dropdown content</Dropdown.Content>
          </Dropdown.Root>
        </Card.Gutter>
      </Card.Header>
      <Card.Content className="text-6xl font-bold">86</Card.Content>
    </Card.Root>
  );
}
