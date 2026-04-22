// Copyright © Spatial Corporation. All rights reserved.

import { Metadata } from "next";
import { ReactNode } from "react";

import { Post } from "../post";

import config from "../config.json";

const posts = config.posts as Post[];

/**
 * Generate metadata for the page.
 */
export async function generateMetadata(props: { params: Promise<{ locale: string; post: string }> }): Promise<Metadata> {
  const params = await props.params;
  const post = posts.find((p) => p.slug === params.post);

  if (!post) {
    return {
      title: "Post not found",
      description: "The post does not exist."
    };
  }

  return {
    title: post.name,
    description: post.description
  };
}

/**
 * Displays a blog post.
 * @param props Configurable options for the element.
 * @returns A post layout element.
 */
export default function Layout(props: { children: ReactNode }) {
  return props.children;
}
