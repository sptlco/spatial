// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Logo, Main } from "@sptlco/design";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  return (
    <Main className="flex flex-col w-full h-screen items-center justify-center gap-10">
      <Logo mode="wordmark" className="h-6 md:h-12 fill-white" />
    </Main>
  );
}
