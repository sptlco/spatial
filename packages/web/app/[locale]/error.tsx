// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect } from "react";

/**
 * A client-side error reporter.
 * @param param0 The error's parameters.
 * @returns An error reporter.
 */
export default function Reporter({ error, reset }: { error: Error & { digest?: string }; reset: () => void }) {
  useEffect(() => {
    // Report the error to Spatial's monitoring service.
    // ...

    console.error("A client error occurred.", error);
  }, [error]);

  return null;
}
