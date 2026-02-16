// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Tabs } from "./tabs";
import { Roles } from "./roles";
import { Users } from "./card";
import { Suspense } from "react";
import { Card } from "@sptlco/design";

export default function Page() {
  return (
    <Card.Root>
      <Card.Header className="px-10">
        <Card.Title className="text-5xl xl:text-7xl font-extrabold">Identity</Card.Title>
        <Card.Description className="xl:text-xl font-light">Grant users access to your computer.</Card.Description>
      </Card.Header>
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
