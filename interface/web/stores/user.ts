// Copyright © Spatial Corporation. All rights reserved.

import { Spatial } from "@sptlco/client";
import { User } from "@sptlco/data";
import { create } from "zustand";
import { devtools } from "zustand/middleware";

type AugmentedUser = User & {
  authenticated: () => boolean;
  can: (scope: string) => boolean;
  is: (role: string) => boolean;
  loading: boolean;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  update: (user: Partial<User>) => void;
};

/**
 * Access the current user.
 */
export const useUser = create<AugmentedUser>()(
  devtools(
    (set, get) => ({
      authenticated: () => !!get().account,
      can: (scope: string) => get().principal?.permissions.includes(scope) ?? false,
      is: (role: string) => get().principal?.roles.includes(role) ?? false,
      loading: true,
      login: async () => {
        set({ loading: true });

        const user = await Spatial.users.me();

        set({
          loading: false,
          account: !user.error ? user.data.account : undefined,
          principal: !user.error ? user.data.principal : undefined,
          session: !user.error ? user.data.session : undefined
        });
      },
      logout: async () => {
        set({ loading: true });

        await Spatial.sessions.destroy();

        set({
          loading: false,
          account: undefined,
          principal: undefined,
          session: undefined
        });
      },
      update: (user) =>
        set((state) => ({
          ...state,
          account: { ...state.account, ...user.account },
          principal: { ...state.principal, ...user.principal },
          session: { ...state.session, ...user.session }
        }))
    }),
    { name: "user" }
  )
);
