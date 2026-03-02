// Copyright © Spatial Corporation. All rights reserved.

import { Metadata } from "./post";

import { Button, Container, createElement, Span } from "@sptlco/design";

/**
 * Displays information about a blog post.
 */
export const Footer = createElement<typeof Container, { metadata: Metadata }>(({ metadata, ...props }, ref) => {
  return (
    <Container className="flex items-center justify-center w-full">
      <Container
        {...props}
        ref={ref}
        className="text-foreground-tertiary font-medium text-xs xl:text-sm flex items-start xl:items-center justify-between w-full xl:max-w-xl"
      >
        <Span className="flex flex-col gap-1 xl:gap-2.5 xl:flex-row xl:items-center">
          <Span>{metadata.author}</Span>
          <Span className="hidden xl:inline">·</Span>
          <Span>{new Date(metadata.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}</Span>
        </Span>
        <Button intent="none" size="fit" className="text-inherit! text-xs! xl:text-sm!">
          Copy link
        </Button>
      </Container>
    </Container>
  );
});
