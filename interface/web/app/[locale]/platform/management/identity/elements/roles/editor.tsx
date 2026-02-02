// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Role } from "@sptlco/data";
import { FormEvent, useState } from "react";

import { Button, Container, createElement, Field, Form, Sheet, toast } from "@sptlco/design";

/**
 * An element that allows for the editing of a role.
 */
export const Editor = createElement<typeof Sheet.Content, { data: Role; onUpdate?: (update: Role) => void }>(
  ({ data: role, onUpdate, ...props }, ref) => {
    const [update, setUpdate] = useState<Role>(role);
    const [updating, setUpdating] = useState(false);

    const edit = async (e: FormEvent) => {
      e.preventDefault();

      setUpdating(true);

      toast.promise(Spatial.roles.update(update), {
        loading: "Updating role",
        description: `We are updating ${role.name}.`,
        success: (response) => {
          setUpdating(false);

          if (!response.error) {
            if (onUpdate) {
              onUpdate(update);
            }

            return {
              message: "Role updated",
              description: "The role has been updated."
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
      <Sheet.Content {...props} ref={ref} title={role.name} description="Edit role display information." closeButton>
        <Form className="flex flex-col w-full sm:w-screen max-w-sm gap-10" onSubmit={edit}>
          <Field
            type="text"
            id="name"
            name="name"
            label="Name"
            value={update.name || ""}
            disabled={updating}
            inset={false}
            autoFocus
            onChange={(e) =>
              setUpdate({
                ...update,
                name: e.target.value
              })
            }
          />
          <Field
            type="text"
            id="description"
            name="description"
            label="Description"
            value={update.description || ""}
            disabled={updating}
            inset={false}
            onChange={(e) =>
              setUpdate({
                ...update,
                description: e.target.value
              })
            }
          />
          <Field
            type="color"
            id="color"
            name="color"
            label="Color"
            value={update.color || ""}
            placeholder="A color code"
            disabled={updating}
            inset={false}
            onValueChange={(color) => setUpdate({ ...update, color })}
          />
          <Field
            type="meta"
            id="metadata"
            name="metadata"
            label="Metadata"
            metadata={update.metadata}
            disabled={updating}
            inset={false}
            onValueChange={(value) => {
              setUpdate({
                ...update,
                metadata: value
              });
            }}
          />
          <Container className="flex items-center gap-4">
            <Button type="submit" disabled={updating}>
              Update
            </Button>
            <Sheet.Close asChild>
              <Button intent="ghost" onClick={() => setUpdate(role)}>
                Cancel
              </Button>
            </Sheet.Close>
          </Container>
        </Form>
      </Sheet.Content>
    );
  }
);
