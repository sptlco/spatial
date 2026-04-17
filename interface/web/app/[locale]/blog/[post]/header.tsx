// Copyright © Spatial Corporation. All rights reserved.

import { Metadata } from "./post";

import { Container, createElement, H1, Image, Link, Logo, Span } from "@sptlco/design";

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
          <Logo mode="symbol" className="h-6 xl:h-8" />
        </Container>
        <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
          <Link href="/blog" className="inline-flex text-inherit hover:text-foreground-primary active:text-foreground-primary">
            Blog
          </Link>
          <Span className="inline-flex mx-2.5">/</Span>
          <Span className="inline-flex text-foreground-primary font-bold">{metadata.topic}</Span>
        </Span>
        <H1 className="text-5xl xl:text-6xl text-center font-semibold">{metadata.name}</H1>
        {metadata.media && <Image className="flex aspect-2/1 object-cover w-full rounded-2xl xl:rounded-4xl" src={metadata.media} />}
        <Span className="text-foreground-tertiary font-medium text-xs xl:text-sm">
          <Span>{metadata.author}</Span>
          <Span className="mx-2.5">·</Span>
          <Span>{new Date(metadata.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}</Span>
        </Span>
      </Container>
    </Container>
  );
});
