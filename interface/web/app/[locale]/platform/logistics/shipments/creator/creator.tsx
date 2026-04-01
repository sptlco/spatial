// Copyright © Spatial Corporation. All rights reserved.

import { Button, createElement, Empty, Icon, Portal, Sheet } from "@sptlco/design";
import { useState } from "react";

export const Creator = createElement<typeof Sheet.Root>((props, ref) => {
  const [parcels, setParcels] = useState<{}[]>([]);

  const add = () => {
    setParcels((prev) => [...prev, {}]);
  };

  const render = () => {
    if (parcels.length === 0) {
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
            <Button onClick={add}>Add Parcel</Button>
          </Empty.Content>
        </Empty.Root>
      );
    }
  };

  return (
    <Portal container={document.body}>
      <Sheet.Root {...props} ref={ref}>
        <Sheet.Content title="Create a Shipment" closeButton className="xl:min-w-1/2">
          {render()}
        </Sheet.Content>
      </Sheet.Root>
    </Portal>
  );
});
