// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Container, Logo, Main } from "@/interface";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  return (
    <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] w-full h-screen gap-10">
      <Container className="row-start-2 col-span-full place-self-center flex flex-col gap-10 items-center justify-center">
        <Logo mode="wordmark" className="h-9 md:h-12 fill-white" />
      </Container>
    </Main>
  );
}
