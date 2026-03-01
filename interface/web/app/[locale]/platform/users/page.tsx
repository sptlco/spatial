// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Header } from "@/elements";
import { Suspense } from "react";

import { Tabs } from "./tabs";
import { Roles } from "./roles";
import { Users } from "./card";

import { Card } from "@sptlco/design";

export default function Page() {
  return (
    <Card.Root>
      <Header title="Identity" description="Grant users access to your computer." />
      <Card.Content>
        <Tabs.Root defaultValue="users">
          <Tabs.List className="px-10">
            <Tabs.Trigger value="users">Users</Tabs.Trigger>
            <Tabs.Trigger value="roles">Roles</Tabs.Trigger>
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
        </Tabs.Root>
      </Card.Content>
    </Card.Root>
  );
}
