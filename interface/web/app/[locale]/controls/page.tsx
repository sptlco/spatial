// Copyright © Spatial Corporation. All rights reserved.

import { Card, Icon } from "@sptlco/design";

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
          <Icon symbol="more_horiz" size={32} />
        </Card.Gutter>
      </Card.Header>
      <Card.Content className="text-6xl font-bold">86</Card.Content>
    </Card.Root>
  );
}
