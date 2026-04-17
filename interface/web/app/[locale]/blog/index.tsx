// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { AnimatePresence, motion, Variants } from "motion/react";
import { useMemo, useState } from "react";

import { Filters } from "./filters";
import { Search } from "./search";
import { Sort } from "./sort";
import { View, ViewType } from "./view";

import { Footer } from "@/elements";
import { Container, createElement, H1, Image, Link, Logo, Main, Pagination, Paragraph, Span } from "@sptlco/design";

import { posts } from "./config.json";

const containerVariants: Variants = {
  hidden: {},
  visible: {
    transition: {
      staggerChildren: 0.06
    }
  },
  exit: {
    transition: {
      staggerChildren: 0.03,
      staggerDirection: -1
    }
  }
};

const gridItemVariants: Variants = {
  hidden: { opacity: 0, y: 18, scale: 0.97 },
  visible: { opacity: 1, y: 0, scale: 1, transition: { duration: 0.3, ease: [0.25, 0.1, 0.25, 1] } },
  exit: { opacity: 0, y: -10, scale: 0.97, transition: { duration: 0.18, ease: "easeIn" } }
};

const listItemVariants: Variants = {
  hidden: { opacity: 0, x: -14 },
  visible: { opacity: 1, x: 0, transition: { duration: 0.28, ease: [0.25, 0.1, 0.25, 1] } },
  exit: { opacity: 0, x: 14, transition: { duration: 0.15, ease: "easeIn" } }
};

/**
 * A public display of blog posts.
 */
export const Index = createElement<typeof Main>((props, ref) => {
  const [view, setView] = useState<ViewType>("grid");
  const [filters, setFilters] = useState<string[]>([]);

  // ...

  const data = useMemo(() => {
    return posts.filter((post) => filters.length === 0 || filters.includes(post.topic));
  }, [posts, filters]);

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
                <Filters selection={filters} onSelectionChange={setFilters} />
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

          <Container className="flex flex-col gap-2 xl:gap-4 text-center">
            <H1 className="text-3xl xl:text-6xl font-bold">Blog</H1>
            <Span className="xl:text-xl text-foreground-tertiary">
              {data.length} post{data.length !== 1 && "s"}
            </Span>
          </Container>

          <AnimatePresence mode="wait">
            <motion.div
              key={view}
              className={clsx("grid gap-10", { "grid-cols-1 sm:grid-cols-2 xl:grid-cols-3": view === "grid" })}
              variants={containerVariants}
              initial="hidden"
              animate="visible"
              exit="exit"
            >
              {data.map((post, i) => {
                switch (view) {
                  case "grid":
                    return (
                      <motion.div key={`grid-${i}`} variants={gridItemVariants}>
                        <Link href={`/blog/${post.slug}`} className="group flex flex-col gap-4 justify-start items-start">
                          <Image src={post.media} />
                          <Span className="font-medium text-lg">{post.name}</Span>
                          <Container className="flex items-center text-sm gap-2">
                            <Span className="font-medium">{post.topic}</Span>
                            <Span
                              className={clsx(
                                "transition-all text-hint whitespace-nowrap",
                                "group-hover:text-foreground-secondary group-active:text-foreground-secondary"
                              )}
                            >
                              {new Date(post.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}
                            </Span>
                          </Container>
                        </Link>
                      </motion.div>
                    );
                  case "list":
                    return (
                      <motion.div key={`list-${i}`} variants={listItemVariants}>
                        <Link
                          href={`/blog/${post.slug}`}
                          className="group flex gap-10 justify-start items-start border-b border-line-faint pb-10 last:p-0 last:border-none"
                        >
                          <Container className="flex flex-col text-sm gap-4 basis-1/5 shrink-0">
                            <Span className="font-medium leading-6">{post.topic}</Span>
                            <Span
                              className={clsx(
                                "transition-all text-hint leading-6 whitespace-nowrap",
                                "group-hover:text-foreground-secondary group-active:text-foreground-secondary"
                              )}
                            >
                              {new Date(post.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}
                            </Span>
                          </Container>
                          <Container className="flex flex-col gap-4 grow">
                            <Span className="font-medium">{post.name}</Span>
                            <Paragraph
                              className={clsx(
                                "transition-all text-hint",
                                "group-hover:text-foreground-secondary group-active:text-foreground-secondary"
                              )}
                            >
                              {post.description}
                            </Paragraph>
                          </Container>
                        </Link>
                      </motion.div>
                    );
                }
              })}
            </motion.div>
          </AnimatePresence>

          <Pagination page={1} pages={1} className="self-center" />
        </Container>
      </Container>
      <Footer />
    </Main>
  );
});
