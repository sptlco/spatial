// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Role } from "@sptlco/data";
import { FormEvent, useState } from "react";

import { Button, Container, createElement, Field, Form, Label, Sheet, Spinner, toast } from "@sptlco/design";

/**
 * Allows the user to create a new role.
 */
export const Creator = createElement<typeof Sheet.Content, { onCreate?: (role: Role) => void }>(({ onCreate, ...props }, ref) => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [color, setColor] = useState("#0364ff");
  const [metadata, setMetadata] = useState<Record<string, string>>();

  const [creating, setCreating] = useState(false);

  const create = async (e: FormEvent) => {
    e.preventDefault();

    setCreating(true);

    toast.promise(Spatial.roles.create({ name, description, color, metadata }), {
      loading: "Creating a role",
      success: (response) => {
        setCreating(false);

        if (!response.error) {
          if (onCreate) {
            onCreate(response.data);
          }

          return {
            message: "Role created",
            description: `Created ${response.data.name} role`
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
    <Sheet.Content {...props} ref={ref} title="Create a new role" description="Grant scoped access to a particular set of users." closeButton>
      <Form className="flex flex-col gap-10 w-full max-w-sm" onSubmit={create}>
        <Field
          type="text"
          id="name"
          name="name"
          label="Name"
          value={name}
          placeholder="A name for the role"
          onChange={(e) => setName(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="text"
          id="description"
          name="description"
          label="Description"
          value={description}
          placeholder="What do these users do?"
          onChange={(e) => setDescription(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="color"
          id="color"
          name="color"
          label="Color"
          value={color || ""}
          placeholder="A color code"
          onValueChange={setColor}
          disabled={creating}
          inset={false}
        />
        <Label className="text-xs text-hint font-extrabold uppercase">Optional</Label>
        <Field
          type="meta"
          id="metadata"
          name="metadata"
          label="Metadata"
          metadata={metadata}
          onValueChange={setMetadata}
          disabled={creating}
          inset={false}
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={creating || !name || !description}>
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
