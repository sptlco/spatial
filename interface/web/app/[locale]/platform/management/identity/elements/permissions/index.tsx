// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { useState } from "react";
import useSWR from "swr";

import { Card, Container, Field, Icon, Span, Spinner, Table } from "@sptlco/design";

/**
 * A dynamic list of permissions.
 * @returns A list of permissions.
 */
export const Permissions = () => {
  const [search, setSearch] = useState("");

  const permissions = useSWR("platform/identity/permissions/list", async (_) => {
    const response = await Spatial.permissions.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const Body = () => {
    if (permissions.isLoading || !permissions.data) {
      return (
        <>
          {[...Array(10)].map((_, i) => (
            <Table.Row key={i}>
              <Table.Cell>
                <Container className="flex items-center gap-5 w-full">
                  <Span className="rounded-full shrink-0 size-12 md:size-16 animate-pulse bg-background-surface" />
                  <Container className="flex flex-col w-full gap-2">
                    <Span className="w-2/3 h-4 rounded-full animate-pulse bg-background-surface" />
                    <Span className="w-4/5 h-4 rounded-full animate-pulse bg-background-surface" />
                  </Container>
                </Container>
              </Table.Cell>
            </Table.Row>
          ))}
        </>
      );
    }

    return (
      <>
        {permissions.data.map((permission, i) => (
          <Table.Row key={i}>
            <Table.Cell />
          </Table.Row>
        ))}
      </>
    );
  };

  return (
    <Card.Root className="bg-background-subtle transform p-10 xl:rounded-4xl">
      <Card.Header>
        <Card.Title className="text-2xl font-bold flex gap-3 items-center">
          <Span>Permissions</Span>
          <Span className="bg-translucent size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
            {permissions.isValidating || !permissions.data ? <Spinner className="size-3 text-foreground-secondary" /> : permissions.data.length}
          </Span>
        </Card.Title>
      </Card.Header>
      <Card.Content className="w-full flex flex-col relative">
        <Container className="relative w-full max-w-sm flex items-center">
          <Field
            type="text"
            id="search"
            name="search"
            placeholder="Search permissions"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full pl-12"
          />
          <Icon symbol="search" className="absolute left-3" />
        </Container>
        <Table.Root className="w-full table-fixed border-separate border-spacing-y-10">
          <Table.Header>
            <Table.Row>
              <Table.Column className="text-left">Permission</Table.Column>
            </Table.Row>
          </Table.Header>
          <Table.Body className="relative">
            <Body />
          </Table.Body>
        </Table.Root>
        {!permissions.data && (
          <Span className="absolute pointer-events-none inset-0 size-full bg-linear-to-b from-transparent to-background-subtle" />
        )}
      </Card.Content>
    </Card.Root>
  );
};
