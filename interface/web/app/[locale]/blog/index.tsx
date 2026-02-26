// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useState } from "react";

import { Filters } from "./filters";
import { Search } from "./search";
import { View, views } from "./view";

import { Container, createElement, Link, Logo, Main, Pagination, ScrollArea } from "@sptlco/design";

/**
 * A public display of blog posts.
 */
export const Index = createElement<typeof ScrollArea.Root>((props, ref) => {
  const [view, setView] = useState(views[0].name);

  return (
    <ScrollArea.Root {...props} ref={ref} className="size-full" fade>
      <ScrollArea.Viewport className="h-screen">
        <Main className="w-full flex flex-col gap-10">
          <Container className="flex w-full p-10 gap-10 items-center justify-between">
            <Link href="/" className="text-inherit!">
              <Logo mode="symbol" className="h-6" />
            </Link>
            <Container className="flex items-center gap-4">
              <View type="single" value={view} onValueChange={setView} />
              <Container className="flex items-center gap-2">
                <Filters />
                <Search />
              </Container>
            </Container>
          </Container>
          <Pagination page={1} pages={1} className="self-center" />
        </Main>
      </ScrollArea.Viewport>
      <ScrollArea.Scrollbar />
    </ScrollArea.Root>
  );
});
