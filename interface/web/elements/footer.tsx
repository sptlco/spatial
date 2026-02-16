// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { createElement, Container, Link, Span } from "@sptlco/design";
import { clsx } from "clsx";
import { useTranslations } from "next-intl";
import useSWR from "swr";

/**
 * A footer displayed at the bottom of the page.
 */
export const Footer = createElement<typeof Container>((props, ref) => {
  const t = useTranslations("footer");

  const version = useSWR("footer/version", async () => {
    const response = await Spatial.version();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  return (
    <Container
      {...props}
      ref={ref}
      className={clsx("w-full flex flex-col sm:flex-row items-center justify-center gap-4 sm:gap-10 text-sm", props.className)}
    >
      <Span className="text-foreground-tertiary inline-flex items-center gap-4">
        <Span
          className={clsx(
            "size-2 rounded-full flex bg-green",
            { "bg-yellow!": version.isLoading || version.isValidating },
            { "bg-red!": version.error }
          )}
        />
        {version.isLoading || version.isValidating ? (
          <Span className="h-4 w-20 rounded-full bg-background-surface animate-pulse" />
        ) : (
          <>Mark {version.data}</>
        )}
      </Span>
    </Container>
  );
});
