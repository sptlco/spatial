// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect } from "react";

import { Button, Container, H1, Icon, Logo, Main, Paragraph, ScrollArea, Span } from "@sptlco/design";

/**
 * A client-side error reporter.
 * @param param0 The error's parameters.
 * @returns An error reporter.
 */
export default function Reporter({ error, reset }: { error: Error & { digest?: string }; reset: () => void }) {
  useEffect(() => {
    // Report the error to Spatial's monitoring service.
    // ...

    console.error("A client error occurred:", error);

    document.documentElement.classList.add("error");

    return () => {
      document.documentElement.classList.remove("error");
    };
  }, [error]);

  return (
    <ScrollArea.Root className="size-full">
      <ScrollArea.Viewport className="max-h-screen">
        <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] p-10 gap-10 w-full min-h-screen text-white">
          <Container className="row-start-2 col-span-full place-self-center flex flex-col items-center justify-center w-full md:max-w-md gap-10">
            <Logo mode="symbol" className="w-12" />
            <H1 className="text-4xl xl:text-6xl font-extrabold">{error.name}</H1>
            <Paragraph className="xl:text-xl text-foreground-secondary text-center">{error.message}</Paragraph>
            {error.stack && <Paragraph className="rounded-xl p-10 bg-black/15">{error.stack}</Paragraph>}
            <Button onClick={reset}>
              <Icon symbol="restart_alt" />
              <Span>Restart</Span>
            </Button>
          </Container>
          {error.digest && <Span className="row-start-3 col-start-2 text-foreground-tertiary">{error.digest}</Span>}
        </Main>
      </ScrollArea.Viewport>
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  );
}
