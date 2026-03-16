// Copyright © Spatial Corporation. All rights reserved.

import { Article, Container, createElement, Separator } from "@sptlco/design";

import { Header } from "./header";
import { Footer } from "./footer";
import { Outline } from "./outline";
import { ScrollIndicator } from "./scroll";
import { Share } from "./share";

import { posts } from "../config.json";

/**
 * Post metadata.
 */
export type Metadata = (typeof posts)[0];

/**
 * A blog post renderer.
 */
export const Post = createElement<typeof Article, { slug: string }>(async ({ slug, ...props }, ref) => {
  const metadata = posts.find((post) => post.slug === slug)!;

  const { default: Markdown } = await import(`@/content/blog/${slug}.mdx`);

  return (
    <Article {...props} ref={ref} className="relative flex flex-col items-center px-10">
      <Header metadata={metadata} />
      <Container className="flex gap-20 w-full max-w-7xl">
        <Container className="flex flex-col gap-10 grow">
          <Markdown />
          <Separator orientation="horizontal" className="w-full h-px bg-line-base" />
          <Footer metadata={metadata} />
        </Container>
        <Container className="hidden xl:flex flex-col gap-10 w-fit min-w-2xs text-sm whitespace-nowrap sticky top-10 self-start">
          <Outline />
          <Share />
        </Container>
      </Container>
      <ScrollIndicator />
    </Article>
  );
});
