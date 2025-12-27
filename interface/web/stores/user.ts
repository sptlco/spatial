// Copyright © Spatial Corporation. All rights reserved.

import { Spatial } from "@sptlco/client";
import { Account, Session } from "@sptlco/data";
import { create } from "zustand";
import { devtools } from "zustand/middleware";

type User = {
  account?: Account;
  authenticated: boolean;
  loading: boolean;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  session?: Session;
};

/**
 * Access the current user.
 */
export const useUser = create<User>()(
  devtools(
    (set) => ({
      loading: true,
      authenticated: false,
      login: async () => {
        set({ loading: true });

        const [account, session] = await Promise.all([Spatial.accounts.me(), Spatial.sessions.me()]);

        set({
          account: !account.error ? account.data : undefined,
          authenticated: !account.error,
          loading: false,
          session: !session.error ? session.data : undefined
        });
      },
      logout: async () => {
        set({ loading: true });

        await Spatial.sessions.destroy();

        set({
          account: undefined,
          authenticated: false,
          loading: false,
          session: undefined
        });
      }
    }),
    { name: "user" }
  )
);
