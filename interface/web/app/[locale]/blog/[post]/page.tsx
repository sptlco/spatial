// Copyright © Spatial Corporation. All rights reserved.

import { Post } from "./post";

/**
 * Renders a blog post.
 * @returns A blog post.
 */
export default async function Page(props: { params: Promise<{ locale: string; post: string }> }) {
  return <Post slug={(await props.params).post} />;
}
