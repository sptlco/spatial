// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
import { FormEvent, useEffect, useState } from "react";
import { getDomain } from "tldts";
import useSWR from "swr";

import { Button, Container, createElement, Field, Form, Icon, Sheet, Span, Spinner, toast } from "@sptlco/design";

export const Creator = createElement<typeof Sheet.Content, { onCreate?: (account: Account) => void }>(({ onCreate, ...props }, ref) => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [domain, setDomain] = useState("");
  const [metadata, setMetadata] = useState<Record<string, string>>();
  const [selectedRoles, setSelectedRoles] = useState<string[]>([]);
  const [creating, setCreating] = useState(false);

  const roles = useSWR("platform/users/creator/roles", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  useEffect(() => {
    setDomain(getDomain(window.location.host) || "");
  }, []);

  const create = async (e: FormEvent) => {
    e.preventDefault();

    setCreating(true);

    toast.promise(Spatial.accounts.create({ name, email, metadata }), {
      loading: "Creating user",
      description: "We are creating a new account with the information you provided.",
      success: async (response) => {
        setCreating(false);

        if (!response.error) {
          await Spatial.assignments.patchMany(
            response.data.id,
            selectedRoles.map((r) => roles.data!.find((d) => d.name === r)!.id)
          );

          if (onCreate) {
            onCreate(response.data);
          }

          return {
            message: "User created",
            description: (
              <>
                Created <Span className="font-semibold">{response.data.name}</Span>.
              </>
            )
          };
        }

        return {
          type: "error",
          message: "Something went wrong",
          description: response.error.message
        };
      }
    });
  };

  return (
    <Sheet.Content {...props} ref={ref} title="New user" description="Grant a user access to your computer." closeButton>
      <Form className="flex flex-col w-full sm:w-screen sm:max-w-sm gap-10" onSubmit={create}>
        <Field
          type="text"
          id="name"
          name="name"
          placeholder="User"
          label="Name"
          value={name}
          onChange={(e) => setName(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="text"
          id="email"
          name="email"
          label="User ID"
          suffix={`@${domain}`}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="meta"
          id="metadata"
          name="metadata"
          label="Metadata"
          metadata={metadata}
          onValueChange={setMetadata}
          disabled={creating}
          inset={false}
          required={false}
        />
        <Field
          type="option"
          id="roles"
          name="roles"
          label="Roles"
          placeholder="Member"
          disabled={creating}
          inset={false}
          required={false}
          multiple
          options={
            roles.data
              ?.sort((a, b) => a.name.localeCompare(b.name))
              .map((role) => ({
                value: role.name,
                label: role.name,
                icon: <Span className="flex size-4 rounded-full" style={{ backgroundColor: role.color }} />,
                chip: (
                  <Span key={role.id} className="flex h-6 items-center gap-2">
                    <Span className="flex size-2 rounded-full" style={{ backgroundColor: role.color }} />
                    <Span className="text-xs font-bold">{role.name}</Span>
                  </Span>
                ),
                description: role.description
              })) ?? []
          }
          selection={selectedRoles}
          onValueChange={(value) =>
            setSelectedRoles((selected) => {
              if (selected.includes(value)) {
                return selected.filter((r) => r !== value);
              }

              return [...selected, value];
            })
          }
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={creating || !name || !email}>
            Create
          </Button>
          <Sheet.Close asChild>
            <Button intent="ghost">Cancel</Button>
          </Sheet.Close>
          {creating && <Spinner className="size-5 text-foreground-secondary" />}
        </Container>
      </Form>
    </Sheet.Content>
  );
});
