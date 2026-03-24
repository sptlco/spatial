// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Suspense } from "react";

import { Application, Tabs } from "@/elements";

import { Assets } from "./assets";
import { Shipments } from "./shipments";

export default function Page() {
  return (
    <Application.Root title="Logistics">
      <Application.Content>
        <Tabs.Root defaultValue="assets">
          <Tabs.List className="px-10">
            <Tabs.Trigger value="assets">Assets</Tabs.Trigger>
            <Tabs.Trigger value="inventory">Inventory</Tabs.Trigger>
            <Tabs.Trigger value="orders">Orders</Tabs.Trigger>
            <Tabs.Trigger value="shipments">Shipments</Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="assets">
            <Suspense>
              <Assets />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="inventory" />
          <Tabs.Content value="orders" />
          <Tabs.Content value="shipments">
            <Suspense>
              <Shipments />
            </Suspense>
          </Tabs.Content>
        </Tabs.Root>
      </Application.Content>
    </Application.Root>
  );
}
