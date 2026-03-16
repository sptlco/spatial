// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { ReactNode, useEffect } from "react";

import { ScrollArea } from "@sptlco/design";

/**
 * A shared layout for all blog pages.
 * @param props Configurable options for the element.
 * @returns A blog layout.
 */
export default function Layout(props: { children: ReactNode }) {
  useEffect(() => {}, []);

  return (
    <ScrollArea.Root className="size-full">
      <ScrollArea.Viewport className="h-screen">{props.children}</ScrollArea.Viewport>
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  );
}
