// Copyright © Spatial Corporation. All rights reserved.

import { Button, Card, Container, Field, Icon, Span } from "@sptlco/design";
import { Tabs } from "./elements";

export default function Page() {
  return (
    <Card.Root>
      <Card.Header>
        <Card.Title className="text-3xl font-extrabold">Identity</Card.Title>
        <Card.Description>Manage users, roles, and permissions.</Card.Description>
      </Card.Header>
      <Card.Content>
        <Tabs.Root defaultValue="users">
          <Tabs.List>
            <Tabs.Trigger value="users">Users</Tabs.Trigger>
            <Tabs.Trigger value="roles">Roles</Tabs.Trigger>
            <Tabs.Trigger value="permissions">Permissions</Tabs.Trigger>
          </Tabs.List>
          <Tabs.Content value="users">
            <Card.Root>
              <Card.Header>
                <Card.Title className="text-2xl font-bold">Users (54)</Card.Title>
                <Card.Gutter>
                  <Button>
                    <Icon symbol="add" />
                    <Span>Create user</Span>
                  </Button>
                  <Button intent="secondary">
                    <Icon symbol="download" />
                    <Span>Export</Span>
                  </Button>
                </Card.Gutter>
              </Card.Header>
              <Card.Content>
                <Container className="relative flex items-center">
                  <Field type="text" id="search" name="search" placeholder="Search users" className="w-full max-w-sm pl-12" />
                  <Icon symbol="search" className="absolute left-3" />
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
