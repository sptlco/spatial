// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect, useState } from "react";

/**
 * Consume information about the current device.
 * @returns Device information.
 */
export const useDevice = () => {
  const [isMobile, setIsMobile] = useState(false);

  useEffect(() => {
    const query = window.matchMedia("(max-width: 768px)");
    const handler = (e: MediaQueryListEvent) => setIsMobile(e.matches);

    setIsMobile(query.matches);

    query.addEventListener("change", handler);
    return () => query.removeEventListener("change", handler);
  }, []);

  return { isMobile };
};
