// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { FC, useEffect } from "react";
import { useShallow } from "zustand/shallow";

/**
 * A component that loads the current user.
 * @returns Nothing.
 */
export const User: FC = () => {
  const user = useUser(
    useShallow((state) => ({
      login: state.login
    }))
  );

  useEffect(() => {
    user.login();
  }, []);

  return null;
};
