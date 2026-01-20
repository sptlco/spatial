// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { Button, Container, createElement, Field, Form, Sheet, toast } from "@sptlco/design";
import { FormEvent, useState } from "react";

/**
 * An element that allows for the editing of a user.
 */
export const Editor = createElement<typeof Sheet.Content, { user: User; onUpdate?: (update: User) => void }>(({ user, onUpdate, ...props }, ref) => {
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

          return {
            message: "Account updated",
            description: "Your account has been updated."
          };
        }

        return {
          message: "Something went wrong",
          description: "An error occurred while updating your account."
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
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={updating}>
            Update
          </Button>
          <Sheet.Close asChild>
            <Button intent="secondary">Cancel</Button>
          </Sheet.Close>
        </Container>
      </Form>
    </Sheet.Content>
  );
});
