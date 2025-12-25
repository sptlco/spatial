// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useUser } from "@/stores";
import { FC, useEffect } from "react";

/**
 * A component that loads the current user.
 * @returns Nothing.
 */
export const User: FC = () => {
  const authenticate = useUser((state) => state.authenticate);

  useEffect(() => {
    authenticate();
  }, []);

  return null;
};
