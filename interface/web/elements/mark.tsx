// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { usePlatform } from "@/utilities";
import { clsx } from "clsx";
import { useTranslations } from "next-intl";

import { createElement, Container, Span } from "@sptlco/design";

/**
 * Displays the current platform version.
 */
export const Mark = createElement<typeof Container>((props, ref) => {
  const t = useTranslations("footer");

  const { __version, version } = usePlatform();

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("w-full flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-10 text-sm font-semibold", props.className)}
    >
      <Span className="cursor-default text-foreground-tertiary inline-flex items-center gap-4">
        <Span
          className={clsx(
            "size-2 rounded-full flex bg-green",
            { "bg-yellow!": __version.isLoading || __version.isValidating },
            { "bg-red!": __version.error }
          )}
        />
        {__version.isLoading || __version.isValidating ? (
          <Span className="h-4 w-20 rounded-full bg-background-surface animate-pulse" />
        ) : (
          <>Mark {version}</>
        )}
      </Span>
    </Container>
  );
});
