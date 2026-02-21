// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { clsx } from "clsx";
import { useEffect, useMemo, useState } from "react";
import { createPortal } from "react-dom";
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
  Combobox,
  Container,
  createElement,
  Dialog,
  Dropdown,
  Field,
  Form,
  Icon,
  LI,
  Pagination,
  Paragraph,
  Sheet,
  Span,
  Spinner,
  Table,
  toast,
  Tooltip,
  UL
} from "@sptlco/design";

const highlight = (text: string, keywords: string[]) => {
  if (keywords.length === 0) {
    return text;
  }

  const escaped = keywords.map((k) => k.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"));
  const regex = new RegExp(`(${escaped.join("|")})`, "gi");

  const parts = text.split(regex);

  return parts.map((part, i) =>
    regex.test(part) ? (
      <mark key={i} className="rounded px-1 bg-yellow text-foreground-inverse">
        {part}
      </mark>
    ) : (
      <span key={i}>{part}</span>
    )
  );
};

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

  const [search, setSearch] = useState("");
  const [selection, setSelection] = useState<string[]>([]);

  const keywords =
    searchParams
      .get("keywords")
      ?.split(",")
      .map((k) => k.trim())
      .filter(Boolean) ?? [];

  const [filters, setFilters] = useState(searchParams.get("filters")?.split(",").filter(Boolean) ?? []);
  const [order, setOrder] = useState(searchParams.get("order")?.split(",").filter(Boolean) ?? []);

  const commitKeywords = (value: string) => {
    const params = new URLSearchParams(searchParams.toString());

    const next = value
      .split(/\s+/)
      .map((k) => k.trim())
      .filter(Boolean);

    if (next.length > 0) {
      params.set("keywords", next.join(","));
      params.delete("users");
    } else {
      params.delete("keywords");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const clearKeywords = () => {
    const params = new URLSearchParams(searchParams.toString());

    params.delete("keywords");
    params.delete("users");

    setSearch("");

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const matchesKeywords = (user: User, keys: string[]) => {
    if (keys.length === 0) {
      return true;
    }

    const haystack = [user.account.name, user.account.email, ...user.principal.roles].join(" ").toLowerCase();

    return keys.some((k) => haystack.includes(k.toLowerCase()));
  };

  const filter = (role: string) => setFilters((value) => (value.includes(role) ? value.filter((r) => r !== role) : [...value, role]));

  const getOrderForProperty = (property: string) => order.find((o) => o.startsWith(`${property}-`));
  const getOrderIndex = (property: string) => order.findIndex((o) => o.startsWith(`${property}-`));

  const toggleSort = (property: string) =>
    setOrder((value) => {
      const asc = `${property}-asc`;
      const desc = `${property}-desc`;

      const ascIndex = value.findIndex((v) => v === asc);
      const descIndex = value.findIndex((v) => v === desc);

      let next = [...value];

      if (ascIndex >= 0) {
        next[ascIndex] = desc;
      } else if (descIndex >= 0) {
        next = next.filter((v) => v !== desc);
      } else {
        next = [...next, asc];
      }

      return next;
    });

  const sort = (users: User[]): User[] => {
    if (order.length === 0) {
      return users;
    }

    return [...users].sort((a, b) => {
      for (const ordering of order) {
        const result = comparators[ordering as Order](a, b);

        if (result !== 0) {
          return result;
        }
      }

      return 0;
    });
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

  const selectOne = (user: User, selected: boolean) => {
    setSelection((s) => [...s.filter((x) => x !== user.account.id), ...(selected ? [user.account.id] : [])]);
  };

  const selectAll = (selected: boolean) => {
    setSelection(selected ? paginatedData.map((u) => u.account.id) : []);
  };

  const downloadMany = (list: User[]) => {
    toast.promise(
      (async () => {
        const json = JSON.stringify(list, null, 2);
        await navigator.clipboard.writeText(json);
      })(),
      {
        loading: `Downloading ${list.length} user${list.length === 1 ? "" : "s"}`,
        success: `Copied ${list.length} user${list.length === 1 ? "" : "s"} to clipboard`,
        error: "Failed to export users"
      }
    );
  };

  const deleteMany = (list: User[]) => {
    toast.promise(Promise.all(list.map((u) => Spatial.accounts.del(u.account.id))), {
      loading: `Deleting ${list.length} user${list.length === 1 ? "" : "s"}`,
      success: async (responses) => {
        const failed = responses.filter((r) => r.error);
        await users.mutate();
        setSelection([]);

        if (failed.length === 0) {
          return {
            message: "Users deleted",
            description: `${list.length} user${list.length === 1 ? "" : "s"} deleted.`
          };
        }

        return {
          type: "error",
          message: "Partial failure",
          description: `${failed.length} user${failed.length === 1 ? "" : "s"} failed to delete.`
        };
      },
      error: {
        message: "Delete failed",
        description: "Something went wrong while deleting users."
      }
    });
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

  const PAGE_SIZE = 20;
  const PAGE = useMemo(() => Math.max(1, Number(searchParams.get("users") ?? 1)), [searchParams]);

  const paginatedData = useMemo(() => {
    const start = (PAGE - 1) * PAGE_SIZE;
    return users.data?.slice(start, start + PAGE_SIZE) ?? [];
  }, [users.data, PAGE]);

  const filteredData =
    (filters.length === 0 ? paginatedData : paginatedData?.filter((u) => filters.every((t) => u.principal.roles.includes(t))))?.filter((u) =>
      matchesKeywords(u, keywords)
    ) ?? [];

  const sortedData = useMemo(() => sort([...filteredData]), [filteredData, order]);
  const pages = Math.ceil(sortedData.length / PAGE_SIZE);

  const selectedUsers = useMemo(() => users.data?.filter((u) => selection.includes(u.account.id)) ?? [], [users.data, selection]);

  const Body = () => {
    if (users.isLoading || !users.data) {
      return (
        <>
          {[...Array(10)].map((_, i) => (
            <Table.Row key={i}>
              <Table.Cell>
                <Span className="flex size-6 rounded-lg animate-pulse bg-background-surface" />
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
                        "rounded-xl animate-pulse bg-background-surface text-sm font-bold inline-flex w-20 h-4 items-center justify-center px-5 py-2 gap-3"
                      )}
                    />
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
          <Row key={user.account.id} user={user} />
        ))}
      </>
    );
  };

  const Row = createElement<typeof Table.Row, { user: User }>(({ user, ...props }, ref) => {
    const [editing, setEditing] = useState(false);
    const [deleting, setDeleting] = useState(false);

    return (
      <Table.Row {...props} ref={ref}>
        <Table.Cell>
          <Checkbox
            className="relative"
            checked={selection.includes(user.account.id)}
            onCheckedChange={(checked: boolean) => selectOne(user, checked)}
          />
        </Table.Cell>
        <Table.Cell>
          <Container onClick={() => setEditing(true)} className="cursor-pointer flex items-center w-full gap-4">
            <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
            <Container className="flex flex-col truncate">
              <Span className="font-semibold truncate">{highlight(user.account.name, keywords)}</Span>
              <Span className="text-sm text-foreground-secondary truncate">{highlight(user.account.email, keywords)}</Span>
            </Container>
            {user.account.id === (account?.id ?? "") && (
              <Span className="hidden xl:flex px-4 py-2 bg-background-highlight rounded-xl text-xs font-bold">You</Span>
            )}
          </Container>
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
                    <Span className="text-foreground-primary font-medium">{highlight(role, keywords)}</Span>
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
              <Dropdown.Item onSelect={() => setEditing(true)}>
                <Dropdown.Icon symbol="person_edit" fill />
                <Span>Edit</Span>
              </Dropdown.Item>
              <Dropdown.Item onSelect={() => downloadMany([user])}>
                <Dropdown.Icon symbol="download" fill />
                <Span>Download</Span>
              </Dropdown.Item>
              <Dropdown.Item onSelect={() => setDeleting(true)}>
                <Dropdown.Icon symbol="close" fill />
                <Span>Delete</Span>
              </Dropdown.Item>
            </Dropdown.Content>
          </Dropdown.Root>
        </Table.Cell>

        <Sheet.Root open={editing} onOpenChange={setEditing}>
          <Editor user={user} onUpdate={(_) => users.mutate()} />
        </Sheet.Root>

        <Dialog.Root open={deleting} onOpenChange={setDeleting}>
          <Dialog.Content title="Delete user" description="Please confirm this action.">
            <Form
              className="flex flex-col gap-10"
              onSubmit={(e) => {
                e.preventDefault();
                deleteMany([user]);
              }}
            >
              <Container className="flex items-center gap-5">
                <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
                <Container className="flex flex-col truncate">
                  <Span className="font-semibold truncate">{user.account.name}</Span>
                  <Span className="text-sm text-foreground-secondary truncate">{user.account.email}</Span>
                </Container>
              </Container>
              <Paragraph className="text-sm text-foreground-secondary">
                This user and all of their account data will be lost immediately upon deletion.
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
  });

  useEffect(() => {
    setSearch(keywords.join(" "));
  }, [searchParams]);

  useEffect(() => {
    if (!selection.every((u) => paginatedData.some((x) => x.account.id === u))) {
      setSelection((v) => v.filter((u) => paginatedData.some((x) => x.account.id === u)));
    }
  }, [paginatedData]);

  const indicator = (property: string) => {
    const ordering = getOrderForProperty(property);
    const index = getOrderIndex(property);
    const direction = ordering?.endsWith("asc") ? (
      <Icon symbol="sort" size={16} className="font-normal -scale-x-100 rotate-90" />
    ) : ordering?.endsWith("desc") ? (
      <Icon symbol="sort" size={16} className="font-normal -rotate-90" />
    ) : null;

    return (
      direction && (
        <Span className="flex items-center justify-center text-hint gap-1">
          {direction}
          {order.length > 1 && <Span className="text-xs font-extrabold">{index + 1}</Span>}
        </Span>
      )
    );
  };

  const [creating, setCreating] = useState(false);

  return (
    <Card.Root>
      <Card.Header className="px-10">
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Users</Span>
          <Span className="bg-translucent shrink-0 size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {users.isValidating || !users.data ? <Spinner className="size-3 text-foreground-secondary" /> : filteredData.length}
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
      <Card.Content className={clsx("w-full flex flex-col relative xl:gap-10!", { "mask-b-from-20% mask-b-to-80%": !users.data })}>
        <Container className="px-10 flex flex-col xl:flex-row w-full items-start xl:items-center gap-5">
          <Form
            className="relative w-full max-w-sm flex items-center"
            onSubmit={(e) => {
              e.preventDefault();
              commitKeywords(search);
            }}
          >
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search users"
              value={search}
              className="w-full pl-12 pr-12"
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Escape" && search.length > 0) {
                  e.preventDefault();
                  clearKeywords();
                }
              }}
            />

            <Icon symbol="search" className="absolute left-3" />

            {keywords.length > 0 ? (
              <Button type="button" intent="ghost" className="absolute right-1 size-8! p-0!" onClick={clearKeywords}>
                <Icon symbol="close" />
              </Button>
            ) : (
              <Button type="submit" intent="ghost" className="absolute right-1 size-8! p-0!">
                <Icon symbol="arrow_forward" />
              </Button>
            )}
          </Form>
          <Container className="flex items-center justify-center w-full xl:w-fit gap-2">
            {roles.isLoading || !roles.data ? (
              <Span className="w-32 h-4 rounded-full bg-background-surface animate-pulse" />
            ) : (
              <Combobox.Root multiple selection={filters} onSelect={filter}>
                <Combobox.Trigger asChild>
                  <Button intent="ghost" className={clsx("data-[state=open]:bg-button-ghost-active")}>
                    <Span>Filter</Span>
                    <Icon symbol="keyboard_arrow_down" />
                    {filters.length > 0 && <Span className="size-2 bg-blue rounded-full flex" />}
                  </Button>
                </Combobox.Trigger>
                <Combobox.Content>
                  <Combobox.List label="Role">
                    {Object.values(roles.data)
                      .sort((a, b) => a.name.localeCompare(b.name))
                      .map((role) => (
                        <Combobox.Item
                          key={role.id}
                          value={role.name}
                          label={role.name}
                          icon={<Span className="flex size-4 rounded-full" style={{ backgroundColor: role.color }} />}
                        />
                      ))}
                  </Combobox.List>
                </Combobox.Content>
              </Combobox.Root>
            )}
            <Combobox.Root multiple selection={order.map((o) => o.replace(/-.*/, ""))} onSelect={toggleSort}>
              <Combobox.Trigger asChild>
                <Button intent="ghost" className={clsx("data-[state=open]:bg-button-ghost-active")}>
                  <Span>Sort</Span>
                  <Icon symbol="keyboard_arrow_down" />
                  {order.length > 0 && <Span className="size-2 bg-blue rounded-full flex" />}
                </Button>
              </Combobox.Trigger>
              <Combobox.Content>
                <Combobox.List label="Field">
                  {Object.entries(properties).map(([key, field]) => {
                    const ordering = getOrderForProperty(key);
                    const index = getOrderIndex(key);
                    const direction = ordering?.endsWith("asc") ? (
                      <Icon symbol="sort" size={16} className="font-normal -scale-x-100 rotate-90" />
                    ) : ordering?.endsWith("desc") ? (
                      <Icon symbol="sort" size={16} className="font-normal -rotate-90" />
                    ) : null;

                    return (
                      <Combobox.Item
                        key={key}
                        value={key}
                        label={field.name}
                        icon={<Icon symbol={field.icon} className="text-hint font-normal" size={20} />}
                        indicator={
                          direction && (
                            <Span className="inline-flex items-center gap-1 text-hint">
                              {direction}
                              {order.length > 1 && <Span className="text-xs font-extrabold">{index + 1}</Span>}
                            </Span>
                          )
                        }
                      />
                    );
                  })}
                </Combobox.List>
              </Combobox.Content>
            </Combobox.Root>
          </Container>
        </Container>
        <Table.Root className="w-full px-10 table-fixed border-separate border-spacing-y-10">
          <Table.Header>
            <Table.Row>
              <Table.Column className="size-12 xl:w-16">
                <Checkbox
                  checked={paginatedData.length > 0 && paginatedData.every((u) => selection.includes(u.account.id))}
                  onCheckedChange={selectAll}
                />
              </Table.Column>
              <Table.Column className="text-left">
                <Span
                  className="cursor-pointer text-sm text-foreground-quaternary font-semibold flex items-center gap-2"
                  onClick={() => toggleSort("name")}
                >
                  <Span>Name</Span>
                  {indicator("name")}
                </Span>
              </Table.Column>
              <Table.Column className="w-md text-left hidden xl:table-cell">
                <Span
                  className="cursor-pointer text-sm text-foreground-quaternary font-semibold flex items-center gap-2"
                  onClick={() => toggleSort("roles")}
                >
                  <Span>Roles</Span>
                  {indicator("roles")}
                </Span>
              </Table.Column>
              <Table.Column className="w-xs text-left hidden xl:table-cell">
                <Span
                  className="cursor-pointer text-sm text-foreground-quaternary font-semibold flex items-center gap-2"
                  onClick={() => toggleSort("created")}
                >
                  <Span>Created</Span>
                  {indicator("created")}
                </Span>
              </Table.Column>
              <Table.Column className="size-12 xl:w-16" />
            </Table.Row>
          </Table.Header>
          <Table.Body className="relative">
            <Body />
          </Table.Body>
        </Table.Root>
        <Pagination page={PAGE} pages={pages} className="self-center" onPageChange={navigate} />
        <Container
          className={clsx("forwards", "opacity-0", "fixed bottom-10  left-1/2 -translate-1/2", "p-10 bg-background-surface shadow-base rounded-2xl", {
            "animate-in slide-in-from-bottom fade-in": selection.length > 0
          })}
        ></Container>
      </Card.Content>
      {selection.length > 0 &&
        createPortal(
          <Container
            className={clsx(
              "bg-blue shadow-base",
              "pointer-events-auto ml-auto flex items-center gap-2 rounded-2xl p-2 animate-in zoom-in-95 slide-in-from-right-50 fade-in duration-500",
              "xl:mx-auto xl:slide-in-from-bottom-50 xl:slide-in-from-right-0"
            )}
          >
            <Tooltip.Root>
              <Tooltip.Trigger asChild>
                <Button intent="ghost" className="size-10! p-0!" onClick={() => downloadMany(selectedUsers)}>
                  <Icon symbol="download" />
                </Button>
              </Tooltip.Trigger>
              <Tooltip.Content side="top" sideOffset={20}>
                Download
              </Tooltip.Content>
            </Tooltip.Root>

            <Dialog.Root>
              <Dialog.Trigger asChild>
                <Container>
                  <Tooltip.Root>
                    <Tooltip.Trigger asChild>
                      <Button intent="ghost" className="size-10! p-0!">
                        <Icon symbol="delete" />
                      </Button>
                    </Tooltip.Trigger>
                    <Tooltip.Content side="top" sideOffset={20}>
                      Delete
                    </Tooltip.Content>
                  </Tooltip.Root>
                </Container>
              </Dialog.Trigger>

              <Dialog.Content
                title={`Delete ${selection.length} user${selection.length === 1 ? "" : "s"}`}
                description="Please confirm this action. This cannot be undone."
              >
                <Form
                  className="flex flex-col gap-10"
                  onSubmit={(e) => {
                    e.preventDefault();
                    deleteMany(selectedUsers);
                  }}
                >
                  <UL className="flex flex-col gap-4 max-h-60 overflow-y-auto">
                    {selectedUsers.map((user) => (
                      <LI key={user.account.id} className="flex items-center gap-4">
                        <Avatar src={user.account.avatar} alt={user.account.name} className="shrink-0 size-12" />
                        <Container className="flex flex-col truncate">
                          <Span className="font-semibold truncate">{user.account.name}</Span>
                          <Span className="text-sm text-foreground-secondary truncate">{user.account.email}</Span>
                        </Container>
                      </LI>
                    ))}
                  </UL>

                  <Paragraph className="text-sm text-foreground-secondary">
                    These accounts and all associated data will be deleted immediately.
                  </Paragraph>

                  <Container className="flex w-full justify-end gap-4">
                    <Dialog.Close asChild>
                      <Button type="button" intent="ghost">
                        Cancel
                      </Button>
                    </Dialog.Close>
                    <Button type="submit" intent="destructive">
                      Delete
                    </Button>
                  </Container>
                </Form>
              </Dialog.Content>
            </Dialog.Root>

            <Tooltip.Root>
              <Tooltip.Trigger asChild>
                <Button intent="ghost" className="size-10! p-0!" onClick={() => setSelection([])}>
                  <Icon symbol="close" fill />
                </Button>
              </Tooltip.Trigger>
              <Tooltip.Content side="top" sideOffset={20}>
                Clear
              </Tooltip.Content>
            </Tooltip.Root>
          </Container>,
          document.getElementById("actions")!
        )}

      <Sheet.Root open={creating} onOpenChange={setCreating}>
        <Creator
          onCreate={(_) => {
            users.mutate();
            setCreating(false);
          }}
        />
      </Sheet.Root>
    </Card.Root>
  );
};

const properties: Record<string, { name: string; icon: string }> = {
  name: {
    name: "Name",
    icon: "match_case"
  },
  roles: {
    name: "Roles",
    icon: "match_case"
  },
  created: {
    name: "Created",
    icon: "123"
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
