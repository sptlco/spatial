// Copyright © Spatial Corporation. All rights reserved.

import { Metadata } from "./post";

import { Container, createElement, H1, Link, Logo, Span } from "@sptlco/design";

/**
 * Displays information about a blog post.
 */
export const Header = createElement<typeof Container, { metadata: Metadata }>(({ metadata, ...props }, ref) => {
  return (
    <Container className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] py-10 w-full min-h-screen">
      <Container
        {...props}
        ref={ref}
        className="row-start-2 col-start-1 col-span-3 place-self-center flex flex-col w-full xl:max-w-5xl items-center gap-10"
      >
        <Container>
          <Logo mode="symbol" className="h-6" />
        </Container>
        <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
          <Link href="/blog" className="inline-flex text-inherit hover:text-foreground-primary active:text-foreground-primary">
            Blog
          </Link>
          <Span className="inline-flex mx-2.5">/</Span>
          <Span className="inline-flex">{metadata.topic}</Span>
        </Span>
        <H1 className="text-3xl xl:text-5xl text-center font-extrabold">{metadata.name}</H1>
        {metadata.media && <Container className="flex aspect-2/1 w-full bg-background-subtle rounded-4xl" />}
        <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
          <Span>{metadata.author}</Span>
          <Span className="mx-2.5">·</Span>
          <Span>{new Date(metadata.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}</Span>
        </Span>
      </Container>
    </Container>
  );
});
