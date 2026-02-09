// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Role } from "@sptlco/data";

import { clsx } from "clsx";
import { useEffect, useMemo, useState } from "react";
import useSWR from "swr";

import { Card, Container, createElement, Form, Icon, LI, Paragraph, Sheet, Span, toast, ToggleGroup, UL } from "@sptlco/design";

/**
 * An element that allows for the editing of permissions.
 */
export const Editor = createElement<typeof Sheet.Content, { data: Role; onUpdate?: (update: Role) => void }>(
  ({ data: role, onUpdate, ...props }, ref) => {
    const permissions = useSWR("platform/identity/permissions/list", async (_) => {
      const response = await Spatial.permissions.list();

      if (response.error) {
        throw response.error;
      }

      return response.data;
    });

    const scopes = useSWR("platform/identity/permissions/scopes/list", async (_) => {
      const response = await Spatial.scopes.list();

      if (response.error) {
        throw response.error;
      }

      return response.data;
    });

    const [initial, setInitial] = useState<string[]>([]);
    const [table, setTable] = useState<string[]>([]);

    const diff = useMemo(() => {
      const added = table.filter((p) => !new Set(initial).has(p)).map((scope) => ({ role: role.id, scope }));
      const removed = initial.filter((p) => !new Set(table).has(p)).map((scope) => ({ role: role.id, scope }));

      return { added, removed };
    }, [table, initial]);

    const [saving, setSaving] = useState(false);

    const could = (scope: string) => new Set(initial).has(scope);
    const can = (scope: string) => new Set(table).has(scope);

    const loading = scopes.isLoading || !scopes.data || permissions.isLoading || !permissions.data;
    const dirty = diff.added.length > 0 || diff.removed.length > 0;

    const toggle = (scope: string, value: boolean) => {
      setTable((t) => {
        if (value) {
          return [...t, scope];
        }

        return t.filter((p) => p !== scope);
      });
    };

    useEffect(() => {
      if (!permissions.data || permissions.isValidating || saving || dirty) {
        return;
      }

      const mine = permissions.data.filter((p) => p.role === role.id).map((p) => p.scope);

      setInitial(mine);
      setTable(mine);
    }, [permissions.data, permissions.isValidating, saving, dirty]);

    useEffect(() => {
      if (!dirty) {
        return;
      }

      const timeout = setTimeout(async () => {
        if (diff.added.length <= 0 && diff.removed.length <= 0) {
          return;
        }

        const count = diff.added.length + diff.removed.length;

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
              description: `Applied ${count} change${count === 1 ? "" : "s"} to permissions.`
            };
          }
        });
      }, 800);

      return () => clearTimeout(timeout);
    }, [diff, table]);

    return (
      <Sheet.Content {...props} ref={ref} title={role.name} description="Grant access to the platform." closeButton>
        <Form className="flex flex-col w-full sm:w-screen max-w-sm gap-10">
          {loading ? (
            <Skeleton />
          ) : (
            scopes.data!.map((sector) => (
              <Card.Root key={sector.name} className="gap-5! w-full">
                <Card.Header
                  className={clsx(
                    "grid-rows-1! py-2.5",
                    "text-hint text-xs uppercase font-bold relative flex items-center",
                    "after:absolute after:w-[calc(100%+80px)] after:h-full after:bg-background-highlight/30 after:-left-10 after:-z-10"
                  )}
                >
                  <Span>{sector.name}</Span>
                </Card.Header>
                <Card.Content className="w-full">
                  <UL className="flex flex-col w-full gap-10">
                    {sector.scopes.map((scope, i) => (
                      <LI key={i} className="flex items-center gap-10">
                        <Container className={clsx("flex flex-col grow gap-2 transition-all", { "opacity-50": !can(scope.tag) })}>
                          <Container className="flex flex-col">
                            <Span className="font-bold">{scope.name}</Span>
                            <Span className="text-xs text-foreground-secondary font-semibold">{scope.tag}</Span>
                          </Container>
                          <Paragraph className="text-hint">{scope.description}</Paragraph>
                        </Container>
                        <ToggleGroup.Root
                          className="rounded-lg shrink-0 overflow-hidden flex bg-input"
                          type="single"
                          value={can(scope.tag) ? "granted" : "revoked"}
                          onValueChange={(value) => toggle(scope.tag, value === "granted")}
                          disabled={saving || !can("permissions.update")}
                        >
                          <ToggleGroup.Item
                            value="revoked"
                            className={clsx(
                              "flex group items-center justify-center size-10 transition-all",
                              "data-[state=on]:bg-red data-[state=on]:text-white"
                            )}
                          >
                            <Icon symbol="block" size={16} className="group-data-[state=on]:font-semibold" />
                          </ToggleGroup.Item>
                          <ToggleGroup.Item
                            value="granted"
                            className={clsx(
                              "flex group items-center justify-center size-10 transition-all",
                              "data-[state=on]:bg-green data-[state=on]:text-background-surface"
                            )}
                          >
                            <Icon symbol="check" size={16} className="group-data-[state=on]:font-semibold" />
                          </ToggleGroup.Item>
                        </ToggleGroup.Root>
                      </LI>
                    ))}
                  </UL>
                </Card.Content>
              </Card.Root>
            ))
          )}
        </Form>
      </Sheet.Content>
    );
  }
);

const Skeleton = () => (
  <Container className="flex flex-col gap-10 relative w-full">
    {[...Array(10)].map((_, i) => (
      <Container key={i} className="flex items-center gap-10 w-full">
        <Container className="flex flex-col gap-2 w-full">
          <Span className="flex w-7/10 h-4 rounded-full bg-translucent animate-pulse" />
          <Span className="flex w-full h-4 rounded-full bg-translucent animate-pulse" />
        </Container>
      </Container>
    ))}
    <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-surface" />
  </Container>
);
