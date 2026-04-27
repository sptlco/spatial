// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useState } from "react";

import { View, ViewType } from "./view";

import { Container, createElement, H2, Pagination, ScrollArea, Table } from "@sptlco/design";

export const Inventory = createElement<typeof Container>((props, ref) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [view, setView] = useState<ViewType>("grid");

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("page", page.toString());
    } else {
      params.delete("page");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const page = 1;
  const pages = 1;

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col w-screen xl:w-auto", props.className)}>
      <Container className="flex flex-col w-full xl:gap-10">
        <Container className="flex flex-col gap-4 px-10 w-full">
          <Container className="flex items-center justify-between">
            <Container className="flex items-center">
              <H2 className="text-2xl font-bold">Assets</H2>
            </Container>
            <View
              type="single"
              value={view}
              onValueChange={(value) => {
                if (value) setView(value as ViewType);
              }}
            />
          </Container>
        </Container>

        <ScrollArea.Root className="w-full" fade fadeOrientation="horizontal">
          <ScrollArea.Viewport className="max-w-full px-10 xl:p-0">
            <Table.Root className="min-w-full table-fixed text-left border-separate border-spacing-y-10"></Table.Root>
          </ScrollArea.Viewport>
          <ScrollArea.Scrollbar orientation="horizontal" className="opacity-0" />
        </ScrollArea.Root>

        <Pagination page={page} pages={pages} className="self-center" onPageChange={navigate} />
      </Container>
    </Container>
  );
});
