// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Main, ScrollArea } from "@sptlco/design";

/**
 * ...
 */
export default function Page() {
  return (
    <ScrollArea.Root className="size-full">
      <ScrollArea.Viewport className="max-h-screen">
        <Main className="p-10" />
      </ScrollArea.Viewport>
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  );
}
