// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Suspense } from "react";
import { Tabs } from "./elements";
import { Roles } from "./elements/roles";
import { Users } from "./elements/users";
import { Card } from "@sptlco/design";

export default function Page() {
  return (
    <Card.Root>
      <Card.Header className="px-10">
        <Card.Title className="text-3xl font-extrabold">Identity</Card.Title>
        <Card.Description>Manage users, roles, and permissions.</Card.Description>
      </Card.Header>
      <Card.Content>
        <Tabs.Root defaultValue="users">
          <Tabs.List className="px-10">
            <Tabs.Trigger value="users">Users</Tabs.Trigger>
            <Tabs.Trigger value="roles">Roles</Tabs.Trigger>
            <Tabs.Trigger value="permissions">Permissions</Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="users" className="xl:pr-10">
            <Suspense>
              <Users />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="roles" className="xl:pr-10">
            <Suspense>
              <Roles />
            </Suspense>
          </Tabs.Content>
          <Tabs.Content value="permissions" className="xl:pr-10"></Tabs.Content>
        </Tabs.Root>
      </Card.Content>
    </Card.Root>
  );
}
