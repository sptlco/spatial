// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { useState } from "react";
import useSWR from "swr";

import { Creator } from "./creator";
import { Editor } from "./editor";

import {
  Button,
  Card,
  Checkbox,
  Container,
  Dialog,
  Dropdown,
  Field,
  Form,
  Icon,
  Monogram,
  Paragraph,
  Sheet,
  Span,
  Spinner,
  Table,
  toast
} from "@sptlco/design";

/**
 * A dynamic list of roles.
 * @returns A list of roles.
 */
export const Roles = () => {
  const [search, setSearch] = useState("");

  const roles = useSWR("platform/identity/roles/list", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const permissions = useSWR("platform/identity/identity/roles/permissions/list", async (_) => {
    const response = await Spatial.permissions.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const Body = () => {
    if (roles.isLoading || !roles.data) {
      return (
        <>
          {[...Array(10)].map((_, i) => (
            <Table.Row key={i}>
              <Table.Cell>
                <Span className="flex size-7 rounded-lg animate-pulse bg-background-surface" />
              </Table.Cell>
              <Table.Cell>
                <Container className="flex items-center gap-5 w-full">
                  <Span className="rounded-full shrink-0 size-12 md:size-16 animate-pulse bg-background-surface" />
                  <Container className="flex flex-col w-full gap-2">
                    <Span className="w-2/3 h-4 rounded-full animate-pulse bg-background-surface" />
                    <Span className="w-4/5 h-4 rounded-full animate-pulse bg-background-surface" />
                  </Container>
                </Container>
              </Table.Cell>
              <Table.Cell>
                <Span className="w-2/3 h-4 rounded-full bg-background-surface animate-pulse" />
              </Table.Cell>
              <Table.Cell />
            </Table.Row>
          ))}
        </>
      );
    }

    return (
      <>
        {roles.data
          .sort((a, b) => (a.name < b.name ? -1 : 1))
          .map((role, i) => (
            <Table.Row key={i}>
              <Table.Cell>
                <Checkbox />
              </Table.Cell>
              <Table.Cell>
                <Container className="flex items-center gap-5">
                  <Monogram text={role.name} className="shrink-0 size-12" style={{ color: role.color }} />
                  <Container className="flex flex-col truncate">
                    <Span className="font-semibold truncate">{role.name}</Span>
                    {role.description && <Span className="text-sm text-foreground-secondary truncate">{role.description}</Span>}
                  </Container>
                </Container>
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell text-center">
                {permissions.isLoading || !permissions.data ? (
                  <Span className="w-2/3 h-4 rounded-full bg-background-surface animate-pulse" />
                ) : (
                  permissions.data.filter((p) => p.role === role.id).length
                )}
              </Table.Cell>
              <Table.Cell>
                <Dropdown.Root>
                  <Dropdown.Trigger asChild>
                    <Button intent="ghost" className="ml-auto! size-10! p-0! data-[state=open]:bg-button-ghost-active">
                      <Icon symbol="more_vert" />
                    </Button>
                  </Dropdown.Trigger>
                  <Dropdown.Content align="end" className="flex flex-col gap-4">
                    <Dropdown.Item asChild>
                      <Sheet.Root>
                        <Sheet.Trigger asChild>
                          <Button intent="ghost" className="w-full" align="left">
                            <Icon symbol="person_edit" />
                            <Span>Edit</Span>
                          </Button>
                        </Sheet.Trigger>
                        <Editor data={role} onUpdate={(_) => roles.mutate()} />
                      </Sheet.Root>
                    </Dropdown.Item>
                    <Dropdown.Item asChild>
                      <Dialog.Root>
                        <Dialog.Trigger asChild>
                          <Button intent="destructive" className="w-full" align="left">
                            <Icon symbol="delete" />
                            <Span>Delete</Span>
                          </Button>
                        </Dialog.Trigger>
                        <Dialog.Content title="Delete role" description="Please confirm this action." className="sm:max-w-md">
                          <Form
                            className="flex flex-col gap-10"
                            onSubmit={(e) => {
                              e.preventDefault();

                              toast.promise(Spatial.roles.del(role.id), {
                                loading: "Deleting role",
                                description: `Deleting role ${role.name}`,
                                success: (response) => {
                                  if (!response.error) {
                                    roles.mutate();

                                    return {
                                      message: "Role deleted",
                                      description: `Deleted role ${role.name}`
                                    };
                                  }

                                  return {
                                    type: "error",
                                    message: "Something went wrong",
                                    description: response.error.message
                                  };
                                }
                              });
                            }}
                          >
                            <Container className="flex items-center gap-5">
                              <Monogram text={role.name} className="shrink-0 size-12" style={{ color: role.color }} />
                              <Container className="flex flex-col truncate">
                                <Span className="font-semibold truncate">{role.name}</Span>
                              </Container>
                            </Container>
                            <Paragraph className="text-sm text-foreground-secondary">
                              Any user with this role assigned will have it removed immediately upon deletion.
                            </Paragraph>
                            <Container className="flex w-full items-center justify-items-start gap-4">
                              <Button type="submit" intent="destructive" className="shrink truncate">
                                Delete
                              </Button>
                            </Container>
                          </Form>
                        </Dialog.Content>
                      </Dialog.Root>
                    </Dropdown.Item>
                  </Dropdown.Content>
                </Dropdown.Root>
              </Table.Cell>
            </Table.Row>
          ))}
      </>
    );
  };

  return (
    <Card.Root className="bg-background-subtle transform p-10 xl:rounded-4xl">
      <Card.Header>
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Roles</Span>
          <Span className="bg-translucent size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {roles.isValidating || !roles.data ? <Spinner className="size-3 text-foreground-secondary" /> : roles.data.length}
          </Span>
        </Card.Title>
        <Card.Gutter className="flex xl:hidden">
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
                      <Span>Create</Span>
                    </Button>
                  </Sheet.Trigger>
                  <Creator onCreate={(_) => roles.mutate()} />
                </Sheet.Root>
              </Dropdown.Item>
            </Dropdown.Content>
          </Dropdown.Root>
        </Card.Gutter>
        <Card.Gutter className="hidden xl:flex">
          <Sheet.Root>
            <Sheet.Trigger asChild>
              <Button>
                <Icon symbol="add" />
                <Span>Create</Span>
              </Button>
            </Sheet.Trigger>
            <Creator onCreate={(_) => roles.mutate()} />
          </Sheet.Root>
        </Card.Gutter>
      </Card.Header>
      <Card.Content className="w-full flex flex-col relative">
        <Container className="relative w-full max-w-sm flex items-center">
          <Field
            type="text"
            id="search"
            name="search"
            placeholder="Search roles"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full pl-12"
          />
          <Icon symbol="search" className="absolute left-3" />
        </Container>
        <Table.Root className="w-full table-fixed border-separate border-spacing-y-10">
          <Table.Header>
            <Table.Row>
              <Table.Column className="w-12 xl:w-16">
                <Checkbox />
              </Table.Column>
              <Table.Column className="text-left">Role</Table.Column>
              <Table.Column className="text-center hidden xl:table-cell">Permissions</Table.Column>
              <Table.Column className="w-12 xl:w-16" />
            </Table.Row>
          </Table.Header>
          <Table.Body className="relative">
            <Body />
          </Table.Body>
        </Table.Root>
        {!roles.data && <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-subtle" />}
      </Card.Content>
    </Card.Root>
  );
};
