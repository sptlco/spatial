// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Tabs, Creator } from "./elements";
import { Button, Card, Container, Dropdown, Field, Icon, Select, Sheet, Span } from "@sptlco/design";
import { useState } from "react";

export default function Page() {
  const [search, setSearch] = useState("");

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
          <Tabs.Content value="users">
            <Card.Root className="bg-background-subtle transform p-10 xl:rounded-4xl">
              <Card.Header>
                <Card.Title className="text-2xl font-bold flex gap-3 items-center">
                  <Span>Users</Span>
                  <Span className="bg-translucent px-4 py-1 rounded-full text-base inline-flex items-center justify-center">124</Span>
                </Card.Title>
                <Card.Gutter className="flex md:hidden">
                  <Dropdown.Root>
                    <Dropdown.Trigger asChild>
                      <Button intent="ghost" className="size-10! p-0! data-[state=open]:bg-button-ghost-active">
                        <Icon symbol="keyboard_arrow_down" />
                      </Button>
                    </Dropdown.Trigger>
                    <Dropdown.Content>
                      <Dropdown.Item asChild>
                        <Sheet.Root>
                          <Sheet.Trigger asChild>
                            <Button intent="ghost" className="w-full" align="left">
                              <Icon symbol="add" />
                              <Span>Create user</Span>
                            </Button>
                          </Sheet.Trigger>
                          <Creator />
                        </Sheet.Root>
                      </Dropdown.Item>
                      <Dropdown.Item asChild>
                        <Button intent="ghost" className="w-full" align="left">
                          <Icon symbol="download" />
                          <Span>Export</Span>
                        </Button>
                      </Dropdown.Item>
                    </Dropdown.Content>
                  </Dropdown.Root>
                </Card.Gutter>
                <Card.Gutter className="hidden md:flex">
                  <Sheet.Root>
                    <Sheet.Trigger asChild>
                      <Button>
                        <Icon symbol="add" />
                        <Span>Create user</Span>
                      </Button>
                    </Sheet.Trigger>
                    <Creator />
                  </Sheet.Root>
                  <Button intent="secondary">
                    <Icon symbol="download" />
                    <Span>Export</Span>
                  </Button>
                </Card.Gutter>
              </Card.Header>
              <Card.Content className="flex md:hidden"></Card.Content>
              <Card.Content className="hidden md:flex">
                <Container className="flex items-center gap-5">
                  <Container className="relative flex items-center">
                    <Field
                      type="text"
                      id="search"
                      name="search"
                      placeholder="Search users"
                      value={search}
                      onChange={(e) => setSearch(e.target.value)}
                      className="w-screen max-w-sm pl-12"
                    />
                    <Icon symbol="search" className="absolute left-3" />
                  </Container>
                  <Span className="text-sm font-bold text-foreground-secondary">Filter by</Span>
                  <Select.Root>
                    <Select.Trigger placeholder="Role" />
                    <Select.Content position="popper"></Select.Content>
                  </Select.Root>
                </Container>
              </Card.Content>
            </Card.Root>
          </Tabs.Content>
          <Tabs.Content value="roles"></Tabs.Content>
          <Tabs.Content value="permissions"></Tabs.Content>
        </Tabs.Root>
      </Card.Content>
    </Card.Root>
  );
}
