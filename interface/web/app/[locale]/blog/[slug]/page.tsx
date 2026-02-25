// Copyright © Spatial Corporation. All rights reserved.

import config from "../config.json";

/**
 * A Markdown page renderer.
 * @param param0 Configurable options for the page.
 * @returns The rendered page.
 */
export default async function Page({ params }: { params: Promise<{ slug: string }> }) {
  const { slug } = await params;
  const { default: Post } = await import(`@/content/${slug}.mdx`);

  return <Post />;
}

/**
 * Pre-render the provided pages.
 * @returns The pages to pre-render.
 */
export function generateStaticParams() {
  return config.posts.map((post) => ({ slug: post.slug }));
}

export const dynamicParams = false;
