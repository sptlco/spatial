// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
import { Button, Container, createElement, Field, Form, Sheet, Spinner, toast } from "@sptlco/design";
import { FormEvent, useState } from "react";

export const Creator = createElement<typeof Sheet.Content, { onCreate?: (account: Account) => void }>(({ onCreate, ...props }, ref) => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");

  const [creating, setCreating] = useState(false);

  const create = async (e: FormEvent) => {
    e.preventDefault();

    setCreating(true);

    toast.promise(Spatial.accounts.create({ name, email }), {
      loading: "Creating an account",
      success: (response) => {
        setCreating(false);

        if (!response.error) {
          if (onCreate) {
            onCreate(response.data);
          }

          return {
            message: "Account created",
            description: `Created user ${response.data.email}`
          };
        }

        return {
          message: "Something went wrong",
          description: "An error occurred while creating the account."
        };
      }
    });
  };

  return (
    <Sheet.Content {...props} ref={ref} title="Create a new user" description="Grant access to a family member, friend, or colleague." closeButton>
      <Form className="flex flex-col gap-10 w-full max-w-sm" onSubmit={create}>
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
          autoFocus
        />
        <Field
          type="text"
          id="email"
          name="email"
          placeholder="name@company.com"
          label="Email address"
          description="The user will sign into the platform with this email address."
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={creating || !name || !email}>
            Create
          </Button>
          <Sheet.Close asChild>
            <Button intent="secondary">Cancel</Button>
          </Sheet.Close>
          {creating && <Spinner className="size-5 text-foreground-secondary" />}
        </Container>
      </Form>
    </Sheet.Content>
  );
});
