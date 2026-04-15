// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import useSWR from "swr";

import { createElement, Container, Span } from "@sptlco/design";

/**
 * Displays the current platform version.
 */
export const Mark = createElement<typeof Container>((props, ref) => {
  const version = useSWR("version", Spatial.version);

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("w-full flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-10 text-sm font-semibold", props.className)}
    >
      <Span className="cursor-default text-foreground-tertiary inline-flex items-center gap-4">
        <Span className={clsx("size-2 rounded-full flex bg-green", { "bg-yellow!": version.isLoading || version.isValidating })} />
        {version.isLoading || version.isValidating || !version.data ? (
          <Span className="h-4 w-20 rounded-full bg-input animate-pulse" />
        ) : (
          <>Mark {version.data}</>
        )}
      </Span>
    </Container>
  );
});
