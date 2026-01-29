// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { FormEvent, useState } from "react";
import { useShallow } from "zustand/shallow";

import { Button, Container, createElement, Field, Form, Sheet, Span, toast } from "@sptlco/design";

/**
 * An element that allows for the editing of a user.
 */
export const Editor = createElement<typeof Sheet.Content, { user: User; onUpdate?: (update: User) => void }>(({ user, onUpdate, ...props }, ref) => {
  const me = useUser(
    useShallow((state) => ({
      account: state.account,
      update: state.update
    }))
  );

  const [update, setUpdate] = useState<User>(user);
  const [updating, setUpdating] = useState(false);

  const edit = async (e: FormEvent) => {
    e.preventDefault();

    setUpdating(true);

    toast.promise(Spatial.accounts.update(update.account), {
      loading: "Updating account",
      description: "We are updating your account with the information you provided.",
      success: (response) => {
        setUpdating(false);

        if (!response.error) {
          if (onUpdate) {
            onUpdate(update);
          }

          if (update.account.id === me.account.id) {
            me.update(update);
          }

          return {
            message: "Account updated",
            description: "Your account has been updated."
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
    <Sheet.Content {...props} ref={ref} title="Edit user" description="Update a user's account details." closeButton>
      <Form className="flex flex-col w-full sm:w-screen max-w-sm gap-10" onSubmit={edit}>
        <Field
          type="text"
          id="name"
          name="name"
          label="Name"
          value={update.account.name || ""}
          disabled={updating}
          inset={false}
          autoFocus
          onChange={(e) =>
            setUpdate({
              ...update,
              account: {
                ...update.account,
                name: e.target.value
              }
            })
          }
        />
        <Field
          type="text"
          id="email"
          name="email"
          label="Email address"
          value={update.account.email || ""}
          disabled={updating}
          inset={false}
          onChange={(e) =>
            setUpdate({
              ...update,
              account: {
                ...update.account,
                email: e.target.value
              }
            })
          }
        />
        <Field
          type="text"
          id="avatar"
          name="avatar"
          label="Avatar (optional)"
          value={update.account.avatar || ""}
          placeholder="An avatar URL"
          disabled={updating}
          inset={false}
          onChange={(e) =>
            setUpdate({
              ...update,
              account: {
                ...update.account,
                avatar: e.target.value
              }
            })
          }
        />
        <Field
          type="meta"
          id="metadata"
          name="metadata"
          label="Metadata"
          metadata={update.account.metadata}
          disabled={updating}
          inset={false}
          onValueChange={(value) => {
            setUpdate({
              ...update,
              account: {
                ...update.account,
                metadata: value
              }
            });
          }}
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={updating}>
            Update
          </Button>
          <Sheet.Close asChild>
            <Button intent="ghost">Cancel</Button>
          </Sheet.Close>
        </Container>
      </Form>
    </Sheet.Content>
  );
});
