// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { clsx } from "clsx";
import { useEffect, useMemo, useState } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useFormatter, useNow } from "next-intl";
import useSWR from "swr";
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
  Monogram,
  Pagination,
  Paragraph,
  Separator,
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
  const now = useNow();
  const format = useFormatter();

  const [selection, setSelection] = useState<string[]>([]);
  const [search, setSearch] = useState("");

  const filters = searchParams.get("filters")?.split(",").filter(Boolean) ?? [];
  const orders = searchParams.get("sort")?.split(",").filter(Boolean) ?? [];

  const filter = (role: string, checked: boolean) => {
    const params = new URLSearchParams(searchParams.toString());
    const next = new Set(filters);

    if (checked) {
      next.add(role);
    } else {
      next.delete(role);
    }

    if (next.size > 0) {
      params.set("filters", Array.from(next).join(","));
    } else {
      params.delete("filters");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const unfilter = () => {
    const params = new URLSearchParams(searchParams.toString());

    params.delete("filters");
    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const getOrderForProperty = (property: string) => orders.find((o) => o.startsWith(`${property}-`));

  const getOrderIndex = (property: string) => orders.findIndex((o) => o.startsWith(`${property}-`));

  const toggleSort = (property: string) => {
    const params = new URLSearchParams(searchParams.toString());
    const next = [...orders];

    const asc = `${property}-asc`;
    const desc = `${property}-desc`;

    const index = next.findIndex((o) => o === asc || o === desc);

    if (index === -1) {
      // OFF → ASC (append, preserving existing priority)
      next.push(asc);
    } else if (next[index] === asc) {
      // ASC → DESC (same priority slot)
      next[index] = desc;
    } else {
      // DESC → OFF (remove entirely)
      next.splice(index, 1);
    }

    if (next.length > 0) {
      params.set("sort", next.join(","));
    } else {
      params.delete("sort");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const sort = (users: User[]): User[] => {
    if (orders.length === 0) return users;

    return [...users].sort((a, b) => {
      for (const order of orders) {
        const result = comparators[order as Order](a, b);

        if (result !== 0) {
          return result;
        }
      }

      return 0;
    });
  };

  const unsort = () => {
    const params = new URLSearchParams(searchParams.toString());

    params.delete("sort");
    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("users", page.toString());
    } else {
      params.delete("users");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const toggle = (user: User, selected: boolean) => {
    setSelection((s) => [...s.filter((x) => x !== user.account.id), ...(selected ? [user.account.id] : [])]);
  };

  const toggleAll = (selected: boolean) => {
    setSelection(selected ? paginatedData.map((u) => u.account.id) : []);
  };

  const { account } = useUser(useShallow((state) => ({ account: state.account })));

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

  const data = (filters.length === 0 ? users.data : users.data?.filter((u) => filters.some((t) => u.principal.roles.includes(t)))) ?? [];
  const sortedData = useMemo(() => sort([...data]), [data, orders]);

  const PAGE_SIZE = 20;

  const page = useMemo(() => Math.max(1, Number(searchParams.get("users") ?? 1)), [searchParams]);
  const pages = Math.ceil(sortedData.length / PAGE_SIZE);

  const paginatedData = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return sortedData.slice(start, start + PAGE_SIZE);
  }, [sortedData, page]);

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
                  <Span className="rounded-full shrink-0 size-12 animate-pulse bg-background-surface" />
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
        {paginatedData.map((user) => (
          <Row key={user.account.id} user={user} />
        ))}
      </>
    );
  };

  const Row = createElement<typeof Table.Row, { user: User }>(({ user, ...props }, ref) => (
    <Table.Row {...props} ref={ref}>
      <Table.Cell>
        <Checkbox checked={selection.includes(user.account.id)} onCheckedChange={(checked: boolean) => toggle(user, checked)} />
      </Table.Cell>
      <Table.Cell>
        <Sheet.Root>
          <Sheet.Trigger asChild>
            <Container className="cursor-pointer flex items-center gap-5">
              <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
              <Container className="flex flex-col truncate">
                <Span className="font-semibold truncate">{user.account.name}</Span>
                <Span className="text-foreground-secondary truncate">{user.account.email}</Span>
              </Container>
              {user.account.id === (account?.id ?? "") && (
                <Span className="hidden xl:flex px-4 py-2 bg-background-highlight rounded-xl text-xs font-bold">You</Span>
              )}
            </Container>
          </Sheet.Trigger>
          <Editor user={user} onUpdate={(_) => users.mutate()} />
        </Sheet.Root>
      </Table.Cell>
      <Table.Cell className="hidden xl:table-cell">
        <UL className="flex flex-wrap gap-4">
          {!roles.isLoading &&
            user.principal.roles
              .sort((a: string, b: string) => (a < b ? -1 : 1))
              .map((role: string, i: number) => (
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
                    <Icon symbol="person_edit" fill />
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
                    <Icon symbol="download" fill />
                    <Span>Export</Span>
                  </Button>
                </Sheet.Trigger>
              </Sheet.Root>
            </Dropdown.Item>
            <Dropdown.Item asChild>
              <Dialog.Root>
                <Dialog.Trigger asChild>
                  <Button intent="ghost" className="w-full" align="left">
                    <Icon symbol="close" fill />
                    <Span>Delete</Span>
                  </Button>
                </Dialog.Trigger>
                <Dialog.Content title="Delete user" description="Please confirm this action.">
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
                              description: (
                                <>
                                  Deleted <Span className="font-semibold">{user.account.name}</Span>.
                                </>
                              )
                            };
                          }

                          return {
                            type: "error",
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
  ));

  useEffect(() => {
    if (!selection.every((u) => paginatedData.some((x) => x.account.id === u))) {
      setSelection((v) => v.filter((u) => paginatedData.some((x) => x.account.id === u)));
    }
  }, [paginatedData]);

  const indicator = (property: string) => {
    const order = getOrderForProperty(property);
    const index = getOrderIndex(property);
    const checked = Boolean(order);

    const direction = order?.endsWith("asc") ? "arrow_upward" : order?.endsWith("desc") ? "arrow_downward" : null;

    return (
      (direction || checked) && (
        <Span className="flex items-center justify-center text-hint gap-0.5">
          {direction && <Icon size={16} symbol={direction} className="font-normal" />}
          {checked && <Span className="text-xs font-extrabold">{index + 1}</Span>}
        </Span>
      )
    );
  };

  return (
    <Card.Root>
      <Card.Header>
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Users</Span>
          <Span className="bg-translucent shrink-0 size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {users.isValidating || !users.data ? <Spinner className="size-3 text-foreground-secondary" /> : data.length}
          </Span>
        </Card.Title>
        <Card.Gutter className="flex xl:hidden">
          {selection.length > 0 && (
            <Dropdown.Root>
              <Dropdown.Trigger asChild>
                <Button intent="ghost" className="px-2!">
                  <Span className="hidden md:flex">Selection</Span>
                  <Span className="text-xs flex items-center justify-center size-6 rounded-full bg-translucent">{selection.length}</Span>
                  <Icon symbol="keyboard_arrow_down" />
                </Button>
              </Dropdown.Trigger>
            </Dropdown.Root>
          )}
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
          {selection.length > 0 && (
            <Dropdown.Root>
              <Dropdown.Trigger asChild>
                <Button intent="ghost">
                  <Span>Selection</Span>
                  <Span className="text-xs flex items-center justify-center size-6 rounded-full bg-translucent">{selection.length}</Span>
                  <Icon symbol="keyboard_arrow_down" />
                </Button>
              </Dropdown.Trigger>
            </Dropdown.Root>
          )}
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
                  <Button intent="ghost" className={clsx("px-5! data-[state=open]:bg-button-ghost-active relative")}>
                    <Span>Filter</Span>
                    <Icon symbol="keyboard_arrow_down" />
                    {filters.length > 0 && <Span className="size-2 bg-blue rounded-full flex" />}
                  </Button>
                </Dropdown.Trigger>
                <Dropdown.Content className="pb-4">
                  <Container className="flex flex-col gap-1">
                    <Dropdown.Label className="px-4 py-2 text-foreground-tertiary font-bold">Role</Dropdown.Label>
                    {Object.values(roles.data)
                      .sort((a, b) => (a.name < b.name ? -1 : 1))
                      .map((role, i) => {
                        const checked = filters.includes(role.name);

                        return (
                          <Dropdown.CheckboxItem
                            key={i}
                            className="group flex items-center gap-4 pr-5"
                            checked={checked}
                            onSelect={(e) => e.preventDefault()}
                            onCheckedChange={(value) => filter(role.name, value)}
                          >
                            <Span className="relative flex items-center justify-center shrink-0 rounded-md size-5">
                              <Dropdown.ItemIndicator className="absolute flex items-center justify-center">
                                <Span className="flex size-2 rounded-full bg-blue" />
                              </Dropdown.ItemIndicator>
                            </Span>
                            <Span className="flex items-center gap-3">
                              <Monogram text={role.name} style={{ color: role.color }} className="size-8" />
                              <Span className="font-bold">{role.name}</Span>
                            </Span>
                          </Dropdown.CheckboxItem>
                        );
                      })}
                  </Container>
                  <Dropdown.Item
                    onSelect={(e) => e.preventDefault()}
                    onClick={unfilter}
                    className={clsx("gap-4 py-5 pointer-events-none bg-transparent!")}
                  >
                    <Span className="font-medium text-hint mx-auto pointer-events-auto">Clear filters</Span>
                  </Dropdown.Item>
                </Dropdown.Content>
              </Dropdown.Root>
            )}
            <Dropdown.Root>
              <Dropdown.Trigger asChild>
                <Button intent="ghost" className="px-5! data-[state=open]:bg-button-ghost-active">
                  <Span>Sort</Span>
                  <Icon symbol="keyboard_arrow_down" />
                  {orders.length > 0 && <Span className="size-2 bg-blue rounded-full flex" />}
                </Button>
              </Dropdown.Trigger>
              <Dropdown.Content className="pb-4">
                <Dropdown.Label className="px-4 py-2 text-foreground-tertiary font-bold">Property</Dropdown.Label>
                {Object.entries(properties).map(([key, field]) => {
                  const order = getOrderForProperty(key);
                  const index = getOrderIndex(key);
                  const checked = Boolean(order);

                  const direction = order?.endsWith("asc") ? "arrow_upward" : order?.endsWith("desc") ? "arrow_downward" : null;

                  return (
                    <Dropdown.CheckboxItem
                      key={key}
                      checked={checked}
                      onSelect={(e) => e.preventDefault()}
                      onCheckedChange={() => toggleSort(key)}
                      className="group flex items-center gap-4"
                    >
                      <Span className="relative flex items-center justify-center shrink-0 rounded-md size-5">
                        <Dropdown.ItemIndicator className="absolute flex items-center justify-center">
                          <Span className="flex size-2 rounded-full bg-blue" />
                        </Dropdown.ItemIndicator>
                      </Span>
                      <Icon size={16} symbol={field.icon} className="text-hint font-light" />
                      <Span className="font-bold">{field.name}</Span>
                      <Span className="flex-1" />
                      {(direction || checked) && (
                        <Span className="flex items-center justify-center gap-0.5">
                          {direction && <Icon size={16} symbol={direction} className="text-hint font-normal" />}
                          {checked && <Span className="text-xs font-extrabold text-hint">{index + 1}</Span>}
                        </Span>
                      )}
                    </Dropdown.CheckboxItem>
                  );
                })}
                <Dropdown.Item
                  onSelect={(e) => e.preventDefault()}
                  onClick={unsort}
                  className={clsx("gap-4 py-5 pointer-events-none bg-transparent!")}
                >
                  <Span className="font-medium text-hint mx-auto pointer-events-auto">Reset order</Span>
                </Dropdown.Item>
              </Dropdown.Content>
            </Dropdown.Root>
          </Container>
        </Container>
        <Table.Root className="w-full table-fixed border-separate border-spacing-y-10">
          <Table.Header>
            <Table.Row>
              <Table.Column className="w-12 xl:w-16">
                <Checkbox
                  checked={paginatedData.length > 0 && paginatedData.every((u) => selection.includes(u.account.id))}
                  onCheckedChange={toggleAll}
                />
              </Table.Column>
              <Table.Column className="text-left">
                <Span className="cursor-pointer flex items-center gap-2" onClick={() => toggleSort("name")}>
                  <Span>Name</Span>
                  {indicator("name")}
                </Span>
              </Table.Column>
              <Table.Column className="w-md text-left hidden xl:table-cell">
                <Span className="cursor-pointer flex items-center gap-2" onClick={() => toggleSort("roles")}>
                  <Span>Roles</Span>
                  {indicator("roles")}
                </Span>
              </Table.Column>
              <Table.Column className="w-xs text-left hidden xl:table-cell">
                <Span className="cursor-pointer flex items-center gap-2" onClick={() => toggleSort("created")}>
                  <Span>Created</Span>
                  {indicator("created")}
                </Span>
              </Table.Column>
              <Table.Column className="w-12 xl:w-16" />
            </Table.Row>
          </Table.Header>
          <Table.Body className="relative">
            <Body />
          </Table.Body>
        </Table.Root>
        <Pagination page={page} pages={pages} className="self-center" onPageChange={navigate} />
        {!users.data && <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-subtle" />}
        <Container
          className={clsx("forwards", "opacity-0", "fixed bottom-10  left-1/2 -translate-1/2", "p-10 bg-background-surface shadow-base rounded-2xl", {
            "animate-in slide-in-from-bottom fade-in": selection.length > 0
          })}
        ></Container>
      </Card.Content>
    </Card.Root>
  );
};

const properties: Record<string, { name: string; icon: string }> = {
  name: {
    name: "Name",
    icon: "text_fields"
  },
  roles: {
    name: "Roles",
    icon: "assignment"
  },
  created: {
    name: "Created",
    icon: "history"
  }
};

type Order = "name-asc" | "name-desc" | "roles-asc" | "roles-desc" | "created-asc" | "created-desc";

const compareRoles = (a: User, b: User) => {
  const ar = [...a.principal.roles].sort();
  const br = [...b.principal.roles].sort();

  const len = Math.min(ar.length, br.length);

  for (let i = 0; i < len; i++) {
    const diff = ar[i].localeCompare(br[i]);
    if (diff !== 0) return diff;
  }

  return ar.length - br.length;
};

const comparators: Record<Order, (a: User, b: User) => number> = {
  "name-asc": (a, b) => a.account.name.localeCompare(b.account.name),
  "name-desc": (a, b) => b.account.name.localeCompare(a.account.name),

  "roles-asc": (a, b) => compareRoles(a, b),
  "roles-desc": (a, b) => compareRoles(b, a),

  "created-asc": (a, b) => a.account.created - b.account.created,
  "created-desc": (a, b) => b.account.created - a.account.created
};
