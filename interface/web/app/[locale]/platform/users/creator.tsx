// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
import { FormEvent, useEffect, useState } from "react";
import { getDomain } from "tldts";

import { Button, Container, createElement, Field, Form, Icon, Sheet, Span, Spinner, toast } from "@sptlco/design";

export const Creator = createElement<typeof Sheet.Content, { onCreate?: (account: Account) => void }>(({ onCreate, ...props }, ref) => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [domain, setDomain] = useState("");
  const [metadata, setMetadata] = useState<Record<string, string>>();
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    setDomain(getDomain(window.location.host) || "");
  }, []);

  const create = async (e: FormEvent) => {
    e.preventDefault();

    setCreating(true);

    toast.promise(Spatial.accounts.create({ name, email, metadata }), {
      loading: "Creating user",
      description: "We are creating a new account with the information you provided.",
      success: (response) => {
        setCreating(false);

        if (!response.error) {
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
    <Sheet.Content {...props} ref={ref} title="New user" description="Grant a user access to Spatial." closeButton>
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
        <Field type="option" id="roles" name="roles" label="Roles" disabled={creating} inset={false} required={false} />
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
