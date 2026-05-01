// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Suspense } from "react";
import { usePathname, useRouter } from "next/navigation";

import { Application, Tabs } from "@/elements";

import { Inventory } from "./inventory";
import { Shipments } from "./shipments";
import { Wallet } from "./wallet";

export default function Page() {
  const router = useRouter();
  const pathname = usePathname();

  const onTabChange = () => {
    router.replace(pathname, { scroll: false });
  };

  return (
    <Application.Root title="Logistics">
      <Application.Content>
        <Tabs.Root defaultValue="inventory" onValueChange={onTabChange}>
          <Tabs.List className="px-10 xl:px-0">
            <Tabs.Trigger value="wallet">Wallet</Tabs.Trigger>
            <Tabs.Trigger value="inventory">Inventory</Tabs.Trigger>
            <Tabs.Trigger value="orders">Orders</Tabs.Trigger>
            <Tabs.Trigger value="shipments">Shipments</Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="wallet">
            <Suspense>
              <Wallet />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="inventory">
            <Suspense>
              <Inventory />
            </Suspense>
          </Tabs.Content>
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
