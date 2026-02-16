// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect } from "react";

/**
 * Mounts the service.
 */
export const Service = () => {
  useEffect(() => {
    if (process.env.NODE_ENV === "production" && "serviceWorker" in navigator) {
      navigator.serviceWorker
        .register("/service.js")
        .then(() => console.log("Registered service worker."))
        .catch(console.error);
    }
  }, []);

  return null;
};
