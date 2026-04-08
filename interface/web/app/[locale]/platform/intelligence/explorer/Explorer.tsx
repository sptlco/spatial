// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "@sptlco/data";
import { Button, Container, createElement, Empty, Icon, Span } from "@sptlco/design";

/**
 * A tree explorer element that allows the user to navigate nodes of the brain.
 */
export const Explorer = createElement<typeof Container, ExplorerProps>(({ entities, ...props }, ref) => {
  const render = () => {
    if (entities.length <= 0) {
      return (
        <Empty.Root className="h-full">
          <Empty.Header>
            <Empty.Media variant="icon">
              <Icon symbol="package_2" />
            </Empty.Media>
            <Empty.Header>No Packages</Empty.Header>
            <Empty.Description>This shipment is currently empty. Get started by adding a parcel.</Empty.Description>
          </Empty.Header>
          <Empty.Content>
            <Button onClick={() => {}}>Add Parcel</Button>
          </Empty.Content>
        </Empty.Root>
      );
    }

    return null;
  };

  return (
    <Container className="flex flex-col relative h-full">
      <Container className="relative transition-all duration-200 overflow-hidden">
        <Container className="flex flex-col w-sm mb-10 rounded-4xl bg-background-surface">
          <Span className="text-foreground-quaternary uppercase py-8 text-center text-xs">Explorer</Span>
          {render()}
        </Container>
      </Container>
    </Container>
  );
});

/**
 * Configurable options for the {@link Explorer}.
 */
export type ExplorerProps = {
  entities: Resource[];
};
