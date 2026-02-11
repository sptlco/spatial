// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useMemo, useState } from "react";
import useSWR from "swr";

import { Creator, Editor as RoleEditor } from ".";
import { Editor as PermissionEditor } from "../permissions";

import {
  Button,
  Card,
  Container,
  Dialog,
  Dropdown,
  Form,
  Icon,
  Monogram,
  Pagination,
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
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const roles = useSWR("platform/identity/roles/list", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const assignments = useSWR("platform/identity/roles/assignments/list", async (_) => {
    const response = await Spatial.assignments.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const permissions = useSWR("platform/identity/roles/permissions/list", async (_) => {
    const response = await Spatial.permissions.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const sortedData = roles.data?.sort((a, b) => (a.name < b.name ? -1 : 1)) ?? [];

  const PAGE_SIZE = 20;

  const page = useMemo(() => Math.max(1, Number(searchParams.get("roles") ?? 1)), [searchParams]);
  const pages = Math.ceil(sortedData.length / PAGE_SIZE);

  const paginatedData = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return sortedData.slice(start, start + PAGE_SIZE);
  }, [sortedData, page]);

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("roles", page.toString());
    } else {
      params.delete("roles");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const [creating, setCreating] = useState(false);

  const Body = () => {
    if (roles.isLoading || !roles.data) {
      return (
        <>
          {[...Array(10)].map((_, i) => (
            <Table.Row key={i}>
              <Table.Cell>
                <Container className="flex items-center gap-5 w-full">
                  <Span className="rounded-full shrink-0 size-12 animate-pulse bg-background-surface" />
                  <Container className="flex flex-col w-full gap-2">
                    <Span className="w-2/3 h-4 rounded-full animate-pulse bg-background-surface" />
                    <Span className="w-4/5 h-4 rounded-full animate-pulse bg-background-surface" />
                  </Container>
                </Container>
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell">
                <Span className="flex mx-auto w-20 h-4 rounded-full bg-background-surface animate-pulse" />
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell">
                <Span className="flex mx-auto w-20 h-4 rounded-full bg-background-surface animate-pulse" />
              </Table.Cell>
              <Table.Cell />
            </Table.Row>
          ))}
        </>
      );
    }

    return (
      <>
        {paginatedData.map((role, i) => {
          const [editing, setEditing] = useState(false);
          const [granting, setGranting] = useState(false);
          const [deleting, setDeleting] = useState(false);

          return (
            <Table.Row key={i}>
              <Table.Cell>
                <Button intent="none" shape="square" size="fit" onClick={() => setEditing(true)} className="text-left">
                  <Monogram text={role.name} className="shrink-0 size-12" style={{ color: role.color }} />
                  <Container className="flex flex-col truncate">
                    <Span className="font-semibold truncate">{role.name}</Span>
                    {role.description && <Span className="text-sm text-foreground-secondary truncate">{role.description}</Span>}
                  </Container>
                </Button>
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell">
                {permissions.isLoading || !permissions.data ? (
                  <Span className="w-2/3 h-4 rounded-full bg-background-surface animate-pulse" />
                ) : (
                  <Button onClick={() => setGranting(true)} intent="none" shape="square" size="fit" className="mx-auto">
                    {permissions.data.filter((p) => p.role === role.id).length}
                  </Button>
                )}
              </Table.Cell>
              <Table.Cell className="hidden xl:table-cell text-center">
                {assignments.isLoading || !assignments.data ? (
                  <Span className="w-2/3 h-4 rounded-full bg-background-surface animate-pulse" />
                ) : (
                  assignments.data.filter((a) => a.role === role.id).length
                )}
              </Table.Cell>
              <Table.Cell>
                <Dropdown.Root>
                  <Dropdown.Trigger asChild>
                    <Button intent="ghost" className="ml-auto! size-10! p-0! data-[state=open]:bg-button-ghost-active">
                      <Icon symbol="more_vert" />
                    </Button>
                  </Dropdown.Trigger>
                  <Dropdown.Content align="end">
                    <Dropdown.Item onSelect={() => setEditing(true)}>
                      <Dropdown.Icon symbol="person_edit" fill />
                      <Span>Edit</Span>
                    </Dropdown.Item>
                    <Dropdown.Item onSelect={() => setGranting(true)}>
                      <Dropdown.Icon symbol="shield_toggle" />
                      <Span>Permissions</Span>
                    </Dropdown.Item>
                    <Dropdown.Item onSelect={() => setDeleting(true)}>
                      <Dropdown.Icon symbol="close" fill />
                      <Span>Delete</Span>
                    </Dropdown.Item>
                  </Dropdown.Content>
                </Dropdown.Root>
              </Table.Cell>

              <Sheet.Root open={editing} onOpenChange={setEditing}>
                <RoleEditor data={role} onUpdate={(_) => roles.mutate()} />
              </Sheet.Root>

              <Sheet.Root
                open={granting}
                onOpenChange={(open) => {
                  setGranting(open);

                  if (!open) {
                    permissions.mutate();
                  }
                }}
              >
                <PermissionEditor data={role} onUpdate={(_) => roles.mutate()} />
              </Sheet.Root>

              <Dialog.Root open={deleting} onOpenChange={setDeleting}>
                <Dialog.Content title="Delete role" description="Please confirm this action.">
                  <Form
                    className="flex flex-col gap-10"
                    onSubmit={(e) => {
                      e.preventDefault();

                      toast.promise(Spatial.roles.del(role.id), {
                        loading: "Deleting role",
                        description: `We are deleting ${role.name}.`,
                        success: (response) => {
                          if (!response.error) {
                            roles.mutate();

                            return {
                              message: "Role deleted",
                              description: `Deleted ${role.name}.`
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
                    <Container className="flex w-full items-center justify-end gap-4">
                      <Dialog.Close asChild>
                        <Button type="button" intent="ghost">
                          Cancel
                        </Button>
                      </Dialog.Close>
                      <Button type="submit" intent="destructive" className="shrink truncate">
                        Delete
                      </Button>
                    </Container>
                  </Form>
                </Dialog.Content>
              </Dialog.Root>
            </Table.Row>
          );
        })}
      </>
    );
  };

  return (
    <>
      <Card.Root className="gap-0!">
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
                <Dropdown.Item onSelect={() => setCreating(true)}>
                  <Icon symbol="add" />
                  <Span>Create</Span>
                </Dropdown.Item>
              </Dropdown.Content>
            </Dropdown.Root>
          </Card.Gutter>
          <Card.Gutter className="hidden xl:flex">
            <Button onClick={() => setCreating(true)}>
              <Icon symbol="add" />
              <Span>Create</Span>
            </Button>
          </Card.Gutter>
        </Card.Header>
        <Card.Content className={clsx("w-full flex flex-col relative", { "mask-b-from-20% mask-b-to-80%": !roles.data })}>
          <Table.Root className="w-full table-fixed border-separate border-spacing-y-10">
            <Table.Header>
              <Table.Row>
                <Table.Column className="text-left">Name</Table.Column>
                <Table.Column className="text-center hidden xl:table-cell">Permissions</Table.Column>
                <Table.Column className="text-center hidden xl:table-cell">Assignments</Table.Column>
                <Table.Column className="w-12 xl:w-16" />
              </Table.Row>
            </Table.Header>
            <Table.Body className="relative">
              <Body />
            </Table.Body>
          </Table.Root>
          <Pagination page={page} pages={pages} className="self-center" onPageChange={navigate} />
        </Card.Content>
      </Card.Root>

      <Sheet.Root open={creating} onOpenChange={setCreating}>
        <Creator onCreate={(_) => roles.mutate()} />
      </Sheet.Root>
    </>
  );
};
