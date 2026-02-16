// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { FormEvent, useState } from "react";
import useSWR from "swr";
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

  const roles = useSWR("platform/users/editor/roles", async (_) => {
    const response = await Spatial.roles.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const [update, setUpdate] = useState<User>(user);
  const [updating, setUpdating] = useState(false);

  const edit = async (e: FormEvent) => {
    e.preventDefault();

    setUpdating(true);

    toast.promise(
      Promise.all([
        Spatial.accounts.update(update.account),
        Spatial.assignments.patchMany(
          update.account.id,
          update.principal.roles.map((r) => roles.data!.find((d) => d.name === r)!.id)
        )
      ]),
      {
        loading: "Updating user",
        description: "We are updating the account with the information you provided.",
        success: (responses) => {
          setUpdating(false);

          if (!responses.some((response) => !!response.error)) {
            if (onUpdate) {
              onUpdate(update);
            }

            if (update.account.id === me.account.id) {
              me.update(update);
            }

            return {
              message: "User updated",
              description: (
                <>
                  Updated <Span className="font-semibold">{user.account.name}</Span>.
                </>
              )
            };
          }

          return {
            type: "error",
            message: "Something went wrong",
            description: responses.find((r) => !!r.error)!.error.message
          };
        }
      }
    );
  };

  return (
    <Sheet.Content {...props} ref={ref} title={user.account.name} description="Update this user's account." closeButton>
      <Form className="flex flex-col w-full sm:w-screen sm:max-w-sm gap-10" onSubmit={edit}>
        <Field
          type="text"
          id="name"
          name="name"
          label="Name"
          value={update.account.name || ""}
          disabled={updating}
          inset={false}
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
        <Field type="text" id="email" name="email" label="User ID" value={user.account.email} readOnly inset={false} />
        <Field
          type="text"
          id="avatar"
          name="avatar"
          label="Avatar"
          value={update.account.avatar || ""}
          placeholder="An avatar URL"
          disabled={updating}
          inset={false}
          required={false}
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
          required={false}
          onValueChange={(value) =>
            setUpdate({
              ...update,
              account: {
                ...update.account,
                metadata: value
              }
            })
          }
        />
        <Field
          type="option"
          id="roles"
          name="roles"
          label="Roles"
          placeholder="Member"
          disabled={updating}
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
          selection={update.principal.roles}
          onValueChange={(value) =>
            setUpdate((u) => {
              if (u.principal.roles.includes(value)) {
                return {
                  ...u,
                  principal: {
                    ...u.principal,
                    roles: u.principal.roles.filter((r) => r !== value)
                  }
                };
              }

              return {
                ...u,
                principal: {
                  ...u.principal,
                  roles: [...u.principal.roles, value]
                }
              };
            })
          }
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={updating || !update.account.name || !update.account.email}>
            Update
          </Button>
          <Sheet.Close asChild>
            <Button intent="ghost" onClick={() => setUpdate(user)}>
              Cancel
            </Button>
          </Sheet.Close>
        </Container>
      </Form>
    </Sheet.Content>
  );
});
