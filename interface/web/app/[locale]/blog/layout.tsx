// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Footer } from "@/elements";
import { ReactNode, useEffect } from "react";

import { Container, ScrollArea } from "@sptlco/design";

/**
 * A shared layout for all blog pages.
 * @param props Configurable options for the element.
 * @returns A blog layout.
 */
export default function Layout(props: { children: ReactNode }) {
  useEffect(() => {}, []);

  return (
    <ScrollArea.Root className="size-full" fade>
      <ScrollArea.Viewport className="h-screen">
        <Container className="flex flex-col gap-10">
          {props.children}
          <Footer />
        </Container>
      </ScrollArea.Viewport>
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  );
}
