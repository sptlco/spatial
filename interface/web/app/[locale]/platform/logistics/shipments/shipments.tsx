// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useEffect, useMemo, useState } from "react";

import { Application } from "@/elements";

import { Button, Container, createElement, Field, Form, Icon, LI, Pagination, Separator, Span, UL } from "@sptlco/design";
import { View, views } from "./view";
import { Filters } from "./filters";
import { Sort } from "./sort";

type Address = {
  name?: string;
  company?: string;
  line1: string;
  line2?: string;
  city: string;
  state: string;
  zip: string;
  country: string;
};

type Shipment = {
  id: string;
  from: Address;
  to: Address;
};

export const Shipments = createElement<typeof Application.Root>((props, ref) => {
  const shipments: Shipment[] = [
    {
      id: "SHP-1055",
      from: {
        company: "Spatial Corporation",
        line1: "240 2ND AVE S",
        line2: "STE 201K",
        city: "Seattle",
        state: "WA",
        zip: "98104",
        country: "USA"
      },
      to: {
        name: "Dakarai Cundiff",
        company: "Spatial Corporation",
        line1: "2014 FAIRVIEW AVE",
        line2: "APT 2211",
        city: "Seattle",
        state: "WA",
        zip: "98121",
        country: "USA"
      }
    }
  ];

  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [search, setSearch] = useState("");
  const [view, setView] = useState(views[0].name);

  const keywords =
    searchParams
      .get("keywords")
      ?.split(",")
      .map((k) => k.trim())
      .filter(Boolean) ?? [];

  const commitKeywords = (value: string) => {
    const params = new URLSearchParams(searchParams.toString());

    const next = value
      .split(/\s+/)
      .map((k) => k.trim())
      .filter(Boolean);

    if (next.length > 0) {
      params.set("keywords", next.join(","));
      params.delete("page");
    } else {
      params.delete("keywords");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const clearKeywords = () => {
    const params = new URLSearchParams(searchParams.toString());

    params.delete("keywords");
    params.delete("page");

    setSearch("");

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("page", page.toString());
    } else {
      params.delete("page");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const PAGE_SIZE = 15;

  const pages = 1;
  const page = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  useEffect(() => {
    setSearch(keywords.join(" "));
  }, [searchParams]);

  const render = () => {
    switch (view) {
      case "grid":
        return (
          <UL className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-3 gap-10">
            {shipments.map((shipment, i) => {
              return [...Array(5)].map((_, j) => (
                <LI key={`${i}+${j}`} className="flex flex-col bg-input outline outline-line-faint rounded-4xl">
                  <Container className="flex flex-col p-10 xl:gap-2">
                    <Container className="flex items-center justify-between">
                      <Span className="text-2xl xl:text-4xl font-bold">{shipment.id}</Span>
                      <Button intent="ghost" className="p-0! size-10!">
                        <Icon symbol="more_horiz" className="font-light" />
                      </Button>
                    </Container>
                    <Span className="text-foreground-quaternary text-xs uppercase">2 Parcels • 2 Units</Span>
                  </Container>
                  <Separator className="flex w-full h-px bg-line-faint" />
                  {renderAddress("Origin", shipment.from)}
                  <Separator className="flex w-full h-px bg-line-faint" />
                  {renderAddress("Destination", shipment.to)}
                </LI>
              ));
            })}
          </UL>
        );
    }
  };

  const renderAddress = (title: string, address: Address) => {
    return (
      <Container className="flex flex-col p-10 gap-5">
        <Span className="flex items-center gap-2.5 text-lg xl:text-2xl font-bold">
          <Span>{title}</Span>
          <Span className="text-foreground-quaternary font-normal">
            {address.city}, {address.state}
          </Span>
        </Span>
        <UL className="flex flex-wrap gap-5 xl:gap-10 w-full">
          {address.name && (
            <LI className="flex flex-col mr-auto">
              <Span className="text-sm mb-2 text-foreground-quaternary">Name</Span>
              <Span className="truncate">{address.name}</Span>
            </LI>
          )}
          {address.company && (
            <LI className="flex flex-col mr-auto">
              <Span className="text-sm mb-2 text-foreground-quaternary">Company</Span>
              <Span className="truncate">{address.company}</Span>
            </LI>
          )}
          <LI className="flex flex-col col-start-3">
            <Span className="text-sm mb-2 text-foreground-quaternary">Address</Span>
            <Span className="truncate">{address.line1}</Span>
            {address.line2 && <Span className="truncate">{address.line2}</Span>}
            <Span className="truncate">
              {address.city}, {address.state} {address.zip}
            </Span>
          </LI>
        </UL>
      </Container>
    );
  };

  return (
    <Application.Root {...props} ref={ref} title="Shipments">
      <Application.Content className="px-10 xl:p-0">
        <Container className="flex flex-col sm:flex-row gap-2">
          <Form
            className="group relative w-full sm:max-w-sm flex items-center"
            onSubmit={(e) => {
              e.preventDefault();
              commitKeywords(search);
            }}
          >
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search shipments"
              value={search}
              className="w-full pl-12 pr-12"
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Escape" && search.length > 0) {
                  e.preventDefault();
                  clearKeywords();
                }
              }}
            />

            <Icon symbol="search" className="absolute left-3" />

            {keywords.length > 0 ? (
              <Button
                type="button"
                onClick={clearKeywords}
                className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5", "group-focus-within:bg-button-highlight!")}
              >
                <Icon symbol="close" />
              </Button>
            ) : (
              <Button
                type="submit"
                className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5", "group-focus-within:bg-button-highlight!")}
              >
                <Icon symbol="arrow_right_alt" size={20} />
              </Button>
            )}
          </Form>
          <Container className="flex w-full sm:grow items-center justify-between gap-2">
            <Container className="flex items-center">
              <Filters />
              <Sort />
            </Container>
            <View
              type="single"
              value={view}
              onValueChange={(value) => {
                if (value) setView(value);
              }}
            />
          </Container>
        </Container>
        {render()}
        <Pagination pages={pages} page={page} className="self-center" onPageChange={navigate} />
      </Application.Content>
    </Application.Root>
  );
});
