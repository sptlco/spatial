// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Filters } from "./filters";
import { Search } from "./search";
import { View, views } from "./view";

import { Navigation } from "@/elements";
import { Container, createElement, Link, Main, Pagination } from "@sptlco/design";

import { posts } from "./config.json";

/**
 * A public display of blog posts.
 */
export const Index = createElement<typeof Main>((props, ref) => {
  const [view, setView] = useState(views[0].name);

  return (
    <>
      <Navigation />
      <Main {...props} ref={ref} className="flex flex-col items-center justify-center">
        <Container className="flex flex-col w-full xl:max-w-7xl gap-10 px-10">
          <Container className="flex w-full items-center justify-end">
            <Container className="flex items-center gap-4">
              <View
                type="single"
                value={view}
                onValueChange={(value) => {
                  if (value) setView(value);
                }}
              />
              <Container className="flex items-center gap-2">
                <Filters />
                <Search />
              </Container>
            </Container>
          </Container>
          {posts
            .filter((p) => p.public)
            .map((post, i) => {
              return (
                <Link key={i} href={`/blog/${post.slug}`}>
                  {post.name}
                </Link>
              );
            })}
          <Pagination page={1} pages={1} className="self-center" />
        </Container>
      </Main>
    </>
  );
});
