// Copyright © Spatial Corporation. All rights reserved.

import { Spatial } from "@sptlco/client";
import { Account, Principal, Session } from "@sptlco/data";
import { create } from "zustand";
import { devtools } from "zustand/middleware";

export type User = {
  account: Account;
  authenticated: () => boolean;
  can: (scope: string) => boolean;
  is: (role: string) => boolean;
  loading: boolean;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  principal: Principal;
  session: Session;
};

/**
 * Access the current user.
 */
export const useUser = create<User>()(
  devtools(
    (set, get) => ({
      authenticated: () => !!get().account,
      can: (scope: string) => get().principal?.permissions.includes(scope) ?? false,
      is: (role: string) => get().principal?.roles.includes(role) ?? false,
      loading: true,
      login: async () => {
        set({ loading: true });

        const [account, principal, session] = await Promise.all([Spatial.accounts.me(), Spatial.principals.me(), Spatial.sessions.me()]);

        set({
          account: !account.error ? account.data : undefined,
          loading: false,
          principal: !principal.error ? principal.data : undefined,
          session: !session.error ? session.data : undefined
        });
      },
      logout: async () => {
        set({ loading: true });

        await Spatial.sessions.destroy();

        set({
          account: undefined,
          loading: false,
          principal: undefined,
          session: undefined
        });
      }
    }),
    { name: "user" }
  )
);
