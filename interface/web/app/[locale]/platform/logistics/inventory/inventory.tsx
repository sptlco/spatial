// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { clsx } from "clsx";
import { AnimatePresence, motion, Variants } from "motion/react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";
import useSWR from "swr";

import { Creator } from "./creator";
import { Filters } from "./filters";
import { Search } from "./search";
import { Sort, SortOrder } from "./sort";
import { View, ViewType } from "./view";

import { Button, Container, createElement, Dropdown, Empty, H2, Icon, Image, Link, Pagination, Paragraph, Sheet, Span } from "@sptlco/design";

const containerVariants: Variants = {
  hidden: {},
  visible: { transition: { staggerChildren: 0.06 } },
  exit: { transition: { staggerChildren: 0.03, staggerDirection: -1 } }
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
  if (keywords.length === 0) return text;

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

const PAGE_SIZE = 12;

export const Inventory = createElement<typeof Container>((props, ref) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [creating, setCreating] = useState(false);

  const [view, setView] = useState<ViewType>("grid");
  const [filters, setFilters] = useState<string[]>([]);
  const [sort, setSort] = useState<SortOrder>("");

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());
    if (page > 1) params.set("page", page.toString());
    else params.delete("page");
    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const keywords = useMemo(() => searchParams.get("keywords")?.trim().split(/\s+/).filter(Boolean) ?? [], [searchParams]);

  const assets = useSWR("platform/logistics/inventory", Spatial.assets.list, {
    refreshInterval: 10000,
    dedupingInterval: 15000
  });

  const data = assets.data ?? [];

  const filtered = useMemo(() => {
    return data.filter((view) => {
      const matchesType = filters.length === 0 || filters.includes(view.asset.type);
      const matchesKeywords =
        keywords.length === 0 ||
        keywords.some((k) => [view.product.name, view.asset.model, view.asset.type].some((field) => field?.toLowerCase().includes(k.toLowerCase())));

      return matchesType && matchesKeywords;
    });
  }, [data, filters, keywords]);

  const sorted = useMemo(() => {
    if (!sort) return filtered;

    const [field, dir] = sort.split("-") as ["name" | "model" | "type" | "quantity", "asc" | "desc"];
    const mul = dir === "asc" ? 1 : -1;

    return [...filtered].sort((a, b) => {
      switch (field) {
        case "name":
          return a.product.name.localeCompare(b.product.name) * mul;
        case "model":
          return a.asset.model.localeCompare(b.asset.model) * mul;
        case "type":
          return a.asset.type.localeCompare(b.asset.type) * mul;
        case "quantity":
          return (a.asset.quantity - b.asset.quantity) * mul;
      }
    });
  }, [filtered, sort]);

  const page = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);
  const pages = Math.ceil(sorted.length / PAGE_SIZE);
  const pagination = useMemo(() => sorted.slice((page - 1) * PAGE_SIZE, page * PAGE_SIZE), [sorted, page]);

  useEffect(() => {
    navigate(1);
  }, [filters, searchParams.get("keywords"), sort]);

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col w-screen xl:w-auto", props.className)}>
      <Container className="flex flex-col w-full xl:gap-10">
        <Container className="flex flex-col gap-4 px-10 w-full">
          <Container className="flex items-center justify-between">
            <Container className="flex items-center gap-4">
              <H2 className="text-2xl font-bold">Assets</H2>
              <Container className="flex items-center">
                <Search />
                <Filters assets={data} selection={filters} onSelectionChange={setFilters} />
                <Sort selection={sort} onSelectionChange={setSort} />
              </Container>
            </Container>
            <Container className="flex items-center xl:gap-5">
              <Container className="flex xl:hidden">
                <Dropdown.Root>
                  <Dropdown.Trigger asChild>
                    <Button intent="ghost" className="size-10! p-0! data-[state=open]:bg-button-ghost-active">
                      <Icon symbol="keyboard_arrow_down" />
                    </Button>
                  </Dropdown.Trigger>
                  <Dropdown.Content>
                    <Dropdown.Item onSelect={() => setCreating(true)}>
                      <Icon symbol="add" />
                      <Span>Create</Span>
                    </Dropdown.Item>
                  </Dropdown.Content>
                </Dropdown.Root>
              </Container>
              <Container className="hidden xl:flex">
                <Button onClick={() => setCreating(true)}>
                  <Icon symbol="add" />
                  <Span>Create</Span>
                </Button>
              </Container>
            </Container>
          </Container>
        </Container>

        <Container className="flex flex-col px-10 py-5 xl:py-0 gap-10">
          <View
            type="single"
            value={view}
            onValueChange={(value) => {
              if (value) setView(value as ViewType);
            }}
          />
          {assets.isLoading || !assets.data ? null : pagination.length === 0 ? (
            <Empty.Root className="my-auto">
              <Empty.Header>
                <Empty.Media variant="icon">
                  <Icon symbol="inventory_2" />
                </Empty.Media>
                <Empty.Title>No assets found</Empty.Title>
                <Empty.Description>
                  {data.length === 0 ? "No assets have been added yet." : "No assets match your search or filters. Try adjusting them."}
                </Empty.Description>
              </Empty.Header>
              <Empty.Content>
                <Button onClick={() => setCreating(true)}>Create Asset</Button>
              </Empty.Content>
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
                  {pagination.map((asset, i) => {
                    switch (view) {
                      case "grid":
                        return (
                          <motion.div key={`grid-${i}`} variants={gridItemVariants}>
                            {/* grid card */}
                          </motion.div>
                        );
                      case "list":
                        return (
                          <motion.div key={`list-${i}`} variants={listItemVariants}>
                            {/* list row */}
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
      <Sheet.Root open={creating} onOpenChange={setCreating}>
        <Creator mutate={assets.mutate} />
      </Sheet.Root>
    </Container>
  );
});
