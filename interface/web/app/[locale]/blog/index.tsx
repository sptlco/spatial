// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { AnimatePresence, motion, Variants } from "motion/react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";

import { Filters } from "./filters";
import { Post } from "./post";
import { Search } from "./search";
import { Sort, SortOrder } from "./sort";
import { View, ViewType } from "./view";

import { Footer } from "@/elements";
import { Container, createElement, Empty, H1, Icon, Image, Link, Logo, Main, Pagination, Paragraph, Span } from "@sptlco/design";

import config from "./config.json";

const posts = config.posts as Post[];

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

const highlight = (text: string, keywords: string[]) => {
  if (keywords.length === 0) {
    return text;
  }

  const escaped = keywords.map((k) => k.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"));
  const regex = new RegExp(`(${escaped.join("|")})`, "gi");
  const parts = text.split(regex);

  return parts.map((part, i) =>
    regex.test(part) ? (
      <mark key={i} className="rounded px-1 bg-yellow text-foreground-inverse">
        {part}
      </mark>
    ) : (
      <span key={i}>{part}</span>
    )
  );
};

/**
 * A public display of blog posts.
 */
export const Index = createElement<typeof Main>((props, ref) => {
  const [view, setView] = useState<ViewType>("grid");
  const [filters, setFilters] = useState<string[]>([]);
  const [sort, setSort] = useState<SortOrder>("");

  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const PAGE_SIZE = 12;

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("page", page.toString());
    } else {
      params.delete("page");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const keywords = useMemo(() => {
    return searchParams.get("keywords")?.trim().split(/\s+/).filter(Boolean) ?? [];
  }, [searchParams]);

  const filtered = useMemo(() => {
    return posts.filter((post) => {
      const matchesTopic = filters.length === 0 || filters.includes(post.topic);
      const matchesKeywords =
        keywords.length === 0 ||
        keywords.some((k) => [post.name, post.topic, post.description].some((field) => field.toLowerCase().includes(k.toLowerCase())));

      return matchesTopic && matchesKeywords;
    });
  }, [posts, filters, keywords]);

  const sorted = useMemo(() => {
    if (!sort) {
      return filtered;
    }

    const [field, dir] = sort.split("-") as ["date" | "name" | "topic", "asc" | "desc"];
    const mul = dir === "asc" ? 1 : -1;

    return [...filtered].sort((a, b) => {
      switch (field) {
        case "date":
          return (new Date(a.date).getTime() - new Date(b.date).getTime()) * mul;
        case "name":
          return a.name.localeCompare(b.name) * mul;
        case "topic":
          return a.topic.localeCompare(b.topic) * mul;
      }
    });
  }, [filtered, sort]);

  const pages = Math.ceil(sorted.length / PAGE_SIZE);
  const page = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  const pagination = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;

    return sorted.slice(start, start + PAGE_SIZE);
  }, [sorted, page]);

  useEffect(() => {
    navigate(1);
  }, [filters, searchParams.get("keywords"), sort]);

  return (
    <Main {...props} ref={ref} className="flex flex-col min-h-screen">
      <Container className="flex flex-col flex-1 items-center py-10">
        <Container className="flex flex-col flex-1 w-full xl:max-w-7xl gap-10 px-10">
          <Container className="flex w-full items-center justify-between">
            <Container className="flex items-center gap-2">
              <Link href="/">
                <Logo mode="symbol" className="h-4" />
              </Link>
              <Container className="flex items-center">
                <Search />
                <Filters selection={filters} onSelectionChange={setFilters} />
                <Sort selection={sort} onSelectionChange={setSort} />
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
          <Container className="flex flex-col gap-2 xl:py-10 xl:gap-4 text-center">
            <H1 className="text-3xl xl:text-7xl font-bold">Blog</H1>
            <Span className="xl:text-xl text-foreground-tertiary">
              {filtered.length} post{filtered.length !== 1 && "s"}
            </Span>
          </Container>
          {pagination.length === 0 ? (
            <Empty.Root className="my-auto">
              <Empty.Header>
                <Empty.Media variant="icon">
                  <Icon symbol="news" />
                </Empty.Media>
                <Empty.Title>No posts found</Empty.Title>
                <Empty.Description>
                  {posts.length === 0
                    ? "Nothing has been published yet. Check back soon."
                    : "No posts match your search or filters. Try adjusting them, or check back later."}
                </Empty.Description>
              </Empty.Header>
            </Empty.Root>
          ) : (
            <>
              <AnimatePresence mode="wait">
                <motion.div
                  key={`${view}-${page}`}
                  className={clsx("grid gap-10", { "grid-cols-1 sm:grid-cols-2 xl:grid-cols-3": view === "grid" })}
                  variants={containerVariants}
                  initial="hidden"
                  animate="visible"
                  exit="exit"
                >
                  {pagination.map((post, i) => {
                    switch (view) {
                      case "grid":
                        return (
                          <motion.div key={`grid-${i}`} variants={gridItemVariants}>
                            <Link href={`/blog/${post.slug}`} className="group flex flex-col gap-4 justify-start items-start">
                              <Container className="overflow-hidden rounded-2xl xl:rounded-4xl">
                                <Image src={post.media} className="object-cover object-center" />
                              </Container>
                              <Span className="font-medium text-lg">{highlight(post.name, keywords)}</Span>
                              <Container className="flex items-center text-sm gap-2">
                                <Span className="font-medium">{highlight(post.topic, keywords)}</Span>
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
                            <Link href={`/blog/${post.slug}`} className="group grid! items-start grid-cols-[8rem_1fr] gap-x-10">
                              <Span className="col-start-1 row-start-1 text-sm font-medium leading-6">{highlight(post.topic, keywords)}</Span>
                              <Span
                                className={clsx(
                                  "col-start-1 row-start-2",
                                  "transition-all text-sm text-hint leading-6 whitespace-nowrap",
                                  "group-hover:text-foreground-secondary group-active:text-foreground-secondary"
                                )}
                              >
                                {new Date(post.date).toLocaleString(undefined, { month: "long", day: "2-digit", year: "numeric" })}
                              </Span>
                              <Span className="col-start-2 row-start-1 font-medium">{highlight(post.name, keywords)}</Span>
                              <Paragraph
                                className={clsx(
                                  "col-start-2 row-start-2",
                                  "transition-all text-hint line-clamp-3",
                                  "group-hover:text-foreground-secondary group-active:text-foreground-secondary"
                                )}
                              >
                                {highlight(post.description, keywords)}
                              </Paragraph>
                            </Link>
                          </motion.div>
                        );
                    }
                  })}
                </motion.div>
              </AnimatePresence>
              <Pagination page={page} pages={pages} className="self-center" onPageChange={navigate} />
            </>
          )}
        </Container>
      </Container>
      <Footer />
    </Main>
  );
});
