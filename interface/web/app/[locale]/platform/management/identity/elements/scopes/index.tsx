// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { Card, Checkbox, Container, Icon, Monogram, ScrollArea, Span, Spinner, Table, toast, Tooltip } from "@sptlco/design";
import { useEffect, useMemo, useState } from "react";

/**
 * A dynamic list of scopes.
 * @returns A list of scopes.
 */
export const Scopes = () => {
  const scopes = useSWR("platform/management/identity/scopes/list", async (_) => {
    const response = await Spatial.scopes.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const roles = useSWR("platform/management/identity/scopes/roles/list", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const permissions = useSWR("platform/management/identity/scopes/permissions/list", async (_) => {
    const response = await Spatial.permissions.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const [initial, setInitial] = useState<{ role: string; scope: string }[]>([]);
  const [table, setTable] = useState<{ role: string; scope: string }[]>([]);

  const diff = useMemo(() => {
    const to = (p: { role: string; scope: string }) => `${p.role}:${p.scope}`;

    const initials = new Set(initial.map(to));
    const tables = new Set(table.map(to));

    const added = table.filter((p) => !initials.has(to(p)));
    const removed = initial.filter((p) => !tables.has(to(p)));

    return { added, removed };
  }, [table, initial]);

  const [saving, setSaving] = useState(false);

  const set = new Set(table.map((p) => `${p.role}:${p.scope}`));
  const can = (role: string, scope: string) => set.has(`${role}:${scope}`);

  const loading = scopes.isLoading || !scopes.data || roles.isLoading || !roles.data || permissions.isLoading || !permissions.data;
  const dirty = diff.added.length > 0 || diff.removed.length > 0;

  const toggle = (role: string, scope: string, value: boolean) => {
    setTable((t) => {
      if (value) {
        return [...t, { role, scope }];
      }

      return t.filter((p) => !(p.role === role && p.scope === scope));
    });
  };

  useEffect(() => {
    if (!permissions.data || permissions.isValidating || saving || dirty) {
      return;
    }

    setInitial(permissions.data);
    setTable(permissions.data);
  }, [permissions.data, permissions.isValidating, saving, dirty]);

  useEffect(() => {
    if (!dirty) {
      return;
    }

    const timeout = setTimeout(async () => {
      setSaving(true);

      toast.promise(Spatial.permissions.update(diff), {
        loading: "Updating permissions",
        description: "",
        success: async (response) => {
          if (response.error) {
            setSaving(false);

            return {
              type: "error",
              message: "Update failed",
              description: response.error.message
            };
          }

          setInitial(table);

          await permissions.mutate();

          setSaving(false);

          return {
            message: "Permissions updated",
            description: "Your changes are being enforced."
          };
        }
      });
    }, 1200);

    return () => clearTimeout(timeout);
  }, [diff, table]);

  return (
    <Card.Root className="bg-background-subtle transform p-10 xl:rounded-4xl gap-0!">
      <Card.Header>
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Scopes</Span>
          <Span className="bg-translucent size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {loading || scopes.isValidating ? (
              <Spinner className="size-3 text-foreground-secondary" />
            ) : (
              scopes.data!.reduce((p, _, i) => p + scopes.data![i].scopes.length, 0)
            )}
          </Span>
        </Card.Title>
      </Card.Header>
      <Card.Content className="w-full flex flex-col relative">
        {loading ? (
          <Table.Root className="border-separate border-spacing-y-10">
            <Table.Header>
              <Table.Row>
                <Table.Column className="w-sm">
                  <Span className="flex rounded-full bg-background-surface animate-pulse w-3/5 h-4" />
                </Table.Column>
                <Table.Column className="hidden xl:table-cell px-10 w-24">
                  <Span className="flex rounded-full bg-background-surface animate-pulse w-full h-4" />
                </Table.Column>
                <Table.Column className="hidden xl:table-cell px-10 w-24">
                  <Span className="flex rounded-full bg-background-surface animate-pulse w-full h-4" />
                </Table.Column>
                <Table.Column className="hidden xl:table-cell px-10 w-24">
                  <Span className="flex rounded-full bg-background-surface animate-pulse w-full h-4" />
                </Table.Column>
                <Table.Column className="hidden xl:table-cell px-10 w-24">
                  <Span className="flex rounded-full bg-background-surface animate-pulse w-full h-4" />
                </Table.Column>
              </Table.Row>
            </Table.Header>
            <Table.Body>
              {[...Array(10)].map((_, i) => (
                <Table.Row key={i}>
                  <Table.Cell>
                    <Container className="flex items-center gap-5 w-full">
                      <Span className="flex rounded-full shrink-0 size-12 animate-pulse bg-background-surface" />
                      <Span className="flex w-4/5 h-4 rounded-full animate-pulse bg-background-surface" />
                    </Container>
                  </Table.Cell>
                  <Table.Cell className="hidden xl:table-cell">
                    <Span className="flex mx-auto size-8 rounded-xl bg-background-surface animate-pulse" />
                  </Table.Cell>
                  <Table.Cell className="hidden xl:table-cell">
                    <Span className="flex mx-auto size-8 rounded-xl bg-background-surface animate-pulse" />
                  </Table.Cell>
                  <Table.Cell className="hidden xl:table-cell">
                    <Span className="flex mx-auto size-8 rounded-xl bg-background-surface animate-pulse" />
                  </Table.Cell>
                  <Table.Cell className="hidden xl:table-cell">
                    <Span className="flex mx-auto size-8 rounded-xl bg-background-surface animate-pulse" />
                  </Table.Cell>
                </Table.Row>
              ))}
            </Table.Body>
          </Table.Root>
        ) : (
          scopes.data!.map((sector) => (
            <Card.Root key={sector.name}>
              <Card.Content>
                <ScrollArea.Root>
                  <ScrollArea.Viewport className="max-w-[calc(100vw-80px)]">
                    <Table.Root className="border-separate border-spacing-y-10">
                      <Table.Header>
                        <Table.Row>
                          <Table.Column className="xl:min-w-64 pr-10 xl:pb-10 text-left">
                            <Span className="flex items-center text-xl font-light gap-3">
                              <Span>{sector.name}</Span>
                              <Span className="bg-translucent size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
                                {sector.scopes.length}
                              </Span>
                            </Span>
                          </Table.Column>
                          {sector.scopes.map((scope) => (
                            <Table.Column key={scope.tag} className="min-w-32 xl:min-w-64 px-10 xl:pb-10 text-center">
                              <Span className="flex items-center justify-center gap-4">
                                <Span>{scope.name}</Span>
                                {scope.description && (
                                  <Tooltip.Root>
                                    <Tooltip.Trigger asChild>
                                      <Icon symbol="info" className="hidden xl:flex text-hint cursor-pointer font-light" />
                                    </Tooltip.Trigger>
                                    <Tooltip.Content side="bottom" sideOffset={16} align="center" className="rounded-xl max-w-64">
                                      <Span className="flex flex-col items-center gap-4 p-4">
                                        {scope.icon && <Icon symbol={scope.icon} fill className="text-foreground-tertiary" />}
                                        <Span>{scope.description}</Span>
                                      </Span>
                                    </Tooltip.Content>
                                  </Tooltip.Root>
                                )}
                              </Span>
                            </Table.Column>
                          ))}
                        </Table.Row>
                      </Table.Header>
                      <Table.Body>
                        {roles
                          .data!.sort((a, b) => (a.name < b.name ? -1 : 1))
                          .map((role) => (
                            <Table.Row key={role.id}>
                              <Table.Cell className="pr-10 xl:pl-10">
                                <Container className="flex items-center gap-5">
                                  <Monogram text={role.name} className="shrink-0 size-12" style={{ color: role.color }} />
                                  <Container className="flex flex-col truncate">
                                    <Span className="font-semibold truncate">{role.name}</Span>
                                  </Container>
                                </Container>
                              </Table.Cell>
                              {sector.scopes.map((scope) => (
                                <Table.Cell key={scope.tag} className="text-center">
                                  <Container className="flex items-center justify-center">
                                    <Checkbox
                                      checked={can(role.id, scope.tag)}
                                      onCheckedChange={(checked) => toggle(role.id, scope.tag, Boolean(checked))}
                                    />
                                  </Container>
                                </Table.Cell>
                              ))}
                            </Table.Row>
                          ))}
                      </Table.Body>
                    </Table.Root>
                  </ScrollArea.Viewport>
                  <ScrollArea.Scrollbar orientation="horizontal" />
                  <ScrollArea.Corner />
                </ScrollArea.Root>
              </Card.Content>
            </Card.Root>
          ))
        )}
        {loading && <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-subtle" />}
      </Card.Content>
    </Card.Root>
  );
};
