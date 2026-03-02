// Copyright © Spatial Corporation. All rights reserved.

import { Metadata } from "./post";

import { Container, createElement, H1, Link, Logo, Span } from "@sptlco/design";

/**
 * Displays information about a blog post.
 */
export const Header = createElement<typeof Container, { metadata: Metadata }>(({ metadata, ...props }, ref) => {
  return (
    <Container {...props} ref={ref} className="flex flex-col w-full max-w-5xl min-h-screen items-center justify-center gap-6">
      <Container className="pb-10">
        <Logo mode="symbol" className="h-6" />
      </Container>
      <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
        <Link href="/blog" className="inline-flex text-inherit hover:text-foreground-primary active:text-foreground-primary">
          Blog
        </Link>
        <Span className="inline-flex mx-2.5">/</Span>
        <Span className="inline-flex">{metadata.topic}</Span>
      </Span>
      <H1 className="text-2xl xl:text-5xl text-center font-extrabold">{metadata.name}</H1>
      <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
        <Span>{metadata.author}</Span>
        <Span className="mx-2.5">·</Span>
        <Span>{new Date(metadata.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}</Span>
      </Span>
    </Container>
  );
});
