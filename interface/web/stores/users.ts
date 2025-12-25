// Copyright © Spatial Corporation. All rights reserved.

import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
import { create } from "zustand";
import { devtools } from "zustand/middleware";

type UserStore = {
  user?: Account;
  loading: boolean;
  authenticated: boolean;
  authenticate: () => Promise<void>;
};

/**
 * Get access to the user store.
 */
export const useUser = create<UserStore>()(
  devtools(
    (set) => ({
      loading: true,
      authenticated: false,
      authenticate: async () => {
        const response = await Spatial.me();

        if (response.error) {
          set({
            loading: false,
            authenticated: false,
            user: undefined
          });
        } else {
          set({
            loading: false,
            authenticated: true,
            user: response.data
          });
        }
      }
    }),
    { name: "user" }
  )
);
