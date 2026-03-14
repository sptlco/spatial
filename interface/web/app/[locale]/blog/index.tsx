// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Filters } from "./filters";
import { Search } from "./search";
import { Sort } from "./sort";
import { View, views, ViewType } from "./view";

import { Footer } from "@/elements";
import { Container, createElement, Link, Logo, Main, Pagination } from "@sptlco/design";

import { posts } from "./config.json";

/**
 * A public display of blog posts.
 */
export const Index = createElement<typeof Main>((props, ref) => {
  const [view, setView] = useState<ViewType>("grid");

  return (
    <Main {...props} ref={ref}>
      <Container className="flex flex-col items-center justify-center py-10">
        <Container className="flex flex-col w-full xl:max-w-7xl gap-10 px-10">
          <Container className="flex w-full items-center justify-between">
            <Container className="flex items-center gap-2">
              <Link href="/">
                <Logo mode="symbol" className="h-4" />
              </Link>
              <Container className="flex items-center">
                <Search />
                <Filters />
                <Sort />
              </Container>
            </Container>
            <View
              type="single"
              value={view}
              onValueChange={(value) => {
                if (value) setView(value as ViewType);
              }}
            />
          </Container>
          {posts
            .filter((p) => p.public)
            .map((post, i) => {
              switch (view) {
                case "grid":
                  return null;
                case "list":
                  return null;
              }
            })}
          <Pagination page={1} pages={1} className="self-center" />
        </Container>
      </Container>
      <Footer />
    </Main>
  );
});
