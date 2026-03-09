// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Suspense } from "react";

import { Application } from "@/elements";

import { Tabs } from "./tabs";
import { Roles } from "./roles";
import { Users } from "./users";

export default function Page() {
  return (
    <Application.Root title="Identity">
      <Application.Content className="px-10 xl:px-0">
        <Tabs.Root defaultValue="users">
          <Tabs.List>
            <Tabs.Trigger value="users">Users</Tabs.Trigger>
            <Tabs.Trigger value="roles">Roles</Tabs.Trigger>
            <Tabs.Trigger value="settings">Settings</Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="users">
            <Suspense>
              <Users />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="roles">
            <Suspense>
              <Roles />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="settings" />
        </Tabs.Root>
      </Application.Content>
    </Application.Root>
  );
}
