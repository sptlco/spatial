// Copyright © Spatial Corporation. All rights reserved.

import { Article, Container, createElement, Separator } from "@sptlco/design";

import { Header } from "./header";
import { Footer } from "./footer";
import { ScrollIndicator } from "./scroll";

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
    <Article {...props} ref={ref} className="flex flex-col items-center px-10">
      <Header metadata={metadata} />
      <Container className="flex flex-col gap-10 items-center w-full xl:max-w-5xl">
        <Markdown />
        <Separator orientation="horizontal" className="w-full xl:max-w-3xl h-px bg-line-base" />
        <Footer metadata={metadata} />
      </Container>
      <ScrollIndicator />
    </Article>
  );
});
