// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { Role, User } from "@sptlco/data";
import { clsx } from "clsx";
import { memo, useMemo, useState } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useFormatter, useNow } from "next-intl";
import useSWR, { SWRResponse } from "swr";
import { useShallow } from "zustand/shallow";

import { Creator } from "./creator";
import { Editor } from "./editor";

import {
  Avatar,
  Button,
  Card,
  Checkbox,
  Container,
  createElement,
  Dialog,
  Dropdown,
  Field,
  Form,
  Icon,
  LI,
  Paragraph,
  Sheet,
  Span,
  Spinner,
  Table,
  toast,
  UL
} from "@sptlco/design";

/**
 * A dynamic list of users.
 * @returns A list of users.
 */
export const Users = () => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [search, setSearch] = useState("");

  const filter = (role: string, checked: boolean) => {
    const params = new URLSearchParams(searchParams.toString());
    const next = new Set(tags);

    if (checked) {
      next.add(role);
    } else {
      next.delete(role);
    }

    if (next.size > 0) {
      params.set("tags", Array.from(next).join(","));
    } else {
      params.delete("tags");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const users = useSWR("platform/identity/users/list", async (_) => {
    const response = await Spatial.users.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const roles = useSWR("platform/identity/users/roles/list", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return Object.fromEntries(response.data.map((r) => [r.name, r]));
  });

  const tags = searchParams.get("tags")?.split(",").filter(Boolean) ?? [];
  const data =
    (tags.length === 0
      ? users.data
      : users.data?.filter((u) => tags.every((t) => u.principal.roles.includes(t) || (u.account.metadata.type ?? "consumer") === t.toLowerCase()))) ??
    [];

  const sortedData = useMemo(() => [...data].sort((a, b) => b.account.created - a.account.created), [data]);

  const Body = () => {
    if (users.isLoading || !users.data) {
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
              <Table.Cell className="hidden xl:table-cell">
                <UL className="flex flex-wrap gap-4">
                  {[...Array(3)].map((_, i) => (
                    <LI
                      key={i}
                      className={clsx(
                        "rounded-xl animate-pulse bg-background-surface text-xs font-bold inline-flex w-20 h-4 items-center justify-center px-5 py-2 gap-3"
                      )}
                    >
                      <Span className="text-foreground-primary" />
                    </LI>
                  ))}
                </UL>
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell">
                <Span className="flex w-1/2 h-4 rounded-full animate-pulse bg-background-surface" />
              </Table.Cell>
              <Table.Cell />
            </Table.Row>
          ))}
        </>
      );
    }

    return (
      <>
        {sortedData.map((user) => (
          <Row key={user.account.id} user={user} users={users} roles={roles} />
        ))}
      </>
    );
  };

  return (
    <Card.Root className="bg-background-subtle transform p-10 xl:rounded-4xl">
      <Card.Header>
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Users</Span>
          <Span className="bg-translucent size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {users.isValidating || !users.data ? <Spinner className="size-3 text-foreground-secondary" /> : data.length}
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
                  <Creator onCreate={(_) => users.mutate()} />
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
        <Card.Gutter className="hidden xl:flex">
          <Sheet.Root>
            <Sheet.Trigger asChild>
              <Button>
                <Icon symbol="add" />
                <Span>Create</Span>
              </Button>
            </Sheet.Trigger>
            <Creator onCreate={(_) => users.mutate()} />
          </Sheet.Root>
          <Button intent="ghost">
            <Icon symbol="download" />
            <Span>Export</Span>
          </Button>
        </Card.Gutter>
      </Card.Header>
      <Card.Content className="w-full flex flex-col relative">
        <Container className="flex flex-col xl:flex-row w-full items-start xl:items-center gap-5">
          <Container className="relative w-full max-w-sm flex items-center">
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search users"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
              className="w-full pl-12"
            />
            <Icon symbol="search" className="absolute left-3" />
          </Container>
          <Container className="flex items-center justify-center w-full xl:w-fit gap-2">
            {roles.isLoading || !roles.data ? (
              <Span className="w-32 h-4 rounded-full bg-background-surface animate-pulse" />
            ) : (
              <Dropdown.Root>
                <Dropdown.Trigger asChild>
                  <Button intent="ghost" className={clsx("px-5! data-[state=open]:bg-button-ghost-active")}>
                    <Span>Filter</Span>
                    {tags.length > 0 && (
                      <Span className="bg-blue rounded-full size-5 flex items-center justify-center text-xs font-medium">{tags.length}</Span>
                    )}
                    <Icon symbol="keyboard_arrow_down" />
                  </Button>
                </Dropdown.Trigger>
                <Dropdown.Content className="md:max-w-md! pb-4">
                  <Container className="grid md:grid-cols-2">
                    <Container className="flex flex-col gap-1">
                      <Dropdown.Label className="px-4 py-2 text-foreground-tertiary font-bold">Role</Dropdown.Label>
                      {Object.values(roles.data)
                        .sort((a, b) => b.created - a.created)
                        .map((role, i) => {
                          const checked = tags.includes(role.name);

                          return (
                            <Dropdown.CheckboxItem
                              key={i}
                              className="group flex items-center gap-4"
                              checked={checked}
                              onSelect={(e) => e.preventDefault()}
                              onCheckedChange={(value) => filter(role.name, value)}
                            >
                              <Span className="relative flex items-center justify-center shrink-0 bg-translucent group-data-[state=checked]:bg-blue rounded-md size-5">
                                <Dropdown.ItemIndicator className="absolute flex items-center justify-center">
                                  <Icon symbol="check" size={16} className="font-medium" />
                                </Dropdown.ItemIndicator>
                              </Span>
                              <Container className="flex items-center gap-3">
                                <Span className="font-medium">{role.name}</Span>
                              </Container>
                            </Dropdown.CheckboxItem>
                          );
                        })}
                    </Container>
                    <Container className="flex flex-col gap-1">
                      <Dropdown.Label className="px-4 py-2 text-foreground-tertiary font-bold">Type</Dropdown.Label>
                      {["Consumer", "Model"].map((type, i) => {
                        const checked = tags.includes(type);

                        return (
                          <Dropdown.CheckboxItem
                            key={i}
                            className="group flex items-center gap-4"
                            checked={checked}
                            onSelect={(e) => e.preventDefault()}
                            onCheckedChange={(value) => filter(type, value)}
                          >
                            <Span className="relative flex items-center justify-center shrink-0 bg-translucent group-data-[state=checked]:bg-blue rounded-md size-5">
                              <Dropdown.ItemIndicator className="absolute flex items-center justify-center">
                                <Icon symbol="check" size={16} className="font-medium" />
                              </Dropdown.ItemIndicator>
                            </Span>
                            <Container className="flex items-center gap-3">
                              <Span className="font-medium">{type}</Span>
                            </Container>
                          </Dropdown.CheckboxItem>
                        );
                      })}
                    </Container>
                  </Container>
                </Dropdown.Content>
              </Dropdown.Root>
            )}
            <Dropdown.Root>
              <Dropdown.Trigger asChild>
                <Button intent="ghost" className="px-5! data-[state=open]:bg-button-ghost-active">
                  <Span>Sort</Span>
                  <Icon symbol="keyboard_arrow_down" />
                </Button>
              </Dropdown.Trigger>
              <Dropdown.Content>Sort</Dropdown.Content>
            </Dropdown.Root>
          </Container>
        </Container>
        <Table.Root className="w-full table-fixed border-separate border-spacing-y-10">
          <Table.Header>
            <Table.Row>
              <Table.Column className="w-12 xl:w-16">
                <Checkbox />
              </Table.Column>
              <Table.Column className="text-left">User ID</Table.Column>
              <Table.Column className="w-md text-left hidden xl:table-cell">Roles</Table.Column>
              <Table.Column className="w-xs text-left hidden xl:table-cell">Created</Table.Column>
              <Table.Column className="w-12 xl:w-16" />
            </Table.Row>
          </Table.Header>
          <Table.Body className="relative">
            <Body />
          </Table.Body>
        </Table.Root>
        {!users.data && <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-subtle" />}
      </Card.Content>
    </Card.Root>
  );
};

const Row = memo(
  createElement<
    typeof Table.Row,
    {
      user: User;
      users: SWRResponse<User[], any, any>;
      roles: SWRResponse<
        {
          [k: string]: Role;
        },
        any,
        any
      >;
    }
  >(({ user, users, roles, ...props }, ref) => {
    const { account } = useUser(useShallow((state) => ({ account: state.account })));

    const now = useNow();
    const format = useFormatter();

    return (
      <Table.Row {...props} ref={ref}>
        <Table.Cell>
          <Checkbox />
        </Table.Cell>
        <Table.Cell>
          <Container className="flex items-center gap-5">
            <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
            <Container className="flex flex-col truncate">
              <Span className="font-semibold truncate">{user.account.name}</Span>
              <Span className="text-foreground-secondary truncate">{user.account.email}</Span>
            </Container>
            {user.account.id === (account?.id ?? "") && (
              <Span className="hidden xl:flex px-4 py-2 bg-background-highlight rounded-xl text-xs font-bold">You</Span>
            )}
            {user.account.metadata.type === "model" && (
              <Span className="hidden xl:flex px-4 py-2 bg-background-highlight rounded-xl text-xs font-bold">Model</Span>
            )}
          </Container>
        </Table.Cell>
        <Table.Cell className="hidden xl:table-cell">
          <UL className="flex flex-wrap gap-4">
            {!roles.isLoading &&
              user.principal.roles.map((role: string, i: number) => (
                <LI
                  key={i}
                  className="inline-flex w-fit items-center justify-center gap-3 "
                  style={{ color: Object.values(roles.data!).find((r) => r.name == role)?.color ?? "currentColor" }}
                >
                  <Span className="size-2 flex rounded-full bg-current" />
                  <Span className="text-sm text-foreground-primary font-medium">{role}</Span>
                </LI>
              ))}
          </UL>
        </Table.Cell>
        <Table.Cell className="hidden xl:table-cell">
          <Span className="text-foreground-tertiary">{format.relativeTime(new Date(user.account.created), now)}</Span>
        </Table.Cell>
        <Table.Cell>
          <Dropdown.Root>
            <Dropdown.Trigger asChild>
              <Button intent="ghost" className="ml-auto! size-10! p-0! data-[state=open]:bg-button-ghost-active">
                <Icon symbol="more_vert" />
              </Button>
            </Dropdown.Trigger>
            <Dropdown.Content align="end">
              <Dropdown.Item asChild>
                <Sheet.Root>
                  <Sheet.Trigger asChild>
                    <Button intent="ghost" className="w-full" align="left">
                      <Icon symbol="person_edit" />
                      <Span>Edit</Span>
                    </Button>
                  </Sheet.Trigger>
                  <Editor user={user} onUpdate={(_) => users.mutate()} />
                </Sheet.Root>
              </Dropdown.Item>
              <Dropdown.Item asChild>
                <Sheet.Root>
                  <Sheet.Trigger asChild>
                    <Button intent="ghost" className="w-full" align="left">
                      <Icon symbol="download" />
                      <Span>Export</Span>
                    </Button>
                  </Sheet.Trigger>
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
                  <Dialog.Content title="Delete user" description="Please confirm this action." className="sm:max-w-md">
                    <Form
                      className="flex flex-col gap-10"
                      onSubmit={(e) => {
                        e.preventDefault();

                        toast.promise(Spatial.accounts.del(user.account.id), {
                          loading: "Deleting user",
                          description: `Deleting ${user.account.email}`,
                          success: (response) => {
                            if (!response.error) {
                              users.mutate();

                              return {
                                message: "User deleted",
                                description: `Deleted user ${user.account.email}`
                              };
                            }

                            return {
                              message: "Something went wrong",
                              description: "An error occurred while deleting the user."
                            };
                          }
                        });
                      }}
                    >
                      <Container className="flex items-center gap-5">
                        <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
                        <Container className="flex flex-col truncate">
                          <Span className="font-semibold truncate">{user.account.name}</Span>
                          <Span className="text-foreground-secondary truncate">{user.account.email}</Span>
                        </Container>
                      </Container>
                      <Paragraph className="text-sm text-foreground-secondary">
                        This user and all of their account data will be lost immediately upon deletion.
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
    );
  })
);
