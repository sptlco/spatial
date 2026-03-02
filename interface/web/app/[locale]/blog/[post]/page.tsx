// Copyright © Spatial Corporation. All rights reserved.

import { Footer } from "@/elements";
import { Post } from "./post";

import { Container, Main } from "@sptlco/design";

/**
 * Renders a blog post.
 * @returns A blog post.
 */
export default async function Page(props: { params: Promise<{ locale: string; post: string }> }) {
  return (
    <Container className="flex flex-col">
      <Main className="flex flex-col pb-10">
        <Post slug={(await props.params).post} />
      </Main>
      <Footer />
    </Container>
  );
}
