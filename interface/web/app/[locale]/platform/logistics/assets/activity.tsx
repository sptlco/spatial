// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Metric } from "@sptlco/data";
import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useFormatter, useNow } from "next-intl";
import { ReactNode, useEffect, useMemo, useState } from "react";
import useSWR from "swr";

import { Button, Container, createElement, Field, Form, H2, Icon, Link, Pagination, ScrollArea, Span, Table } from "@sptlco/design";

type Field = {
  name: string;
  renderer: (metric: Metric) => ReactNode;
  columnClassName?: string;
  cellClassName?: string;
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
 * An element that displays the current trading objectives.
 */
export const Activity = createElement<typeof Container>((props, ref) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const now = useNow();
  const format = useFormatter();

  const [search, setSearch] = useState("");

  const keywords =
    searchParams
      .get("keywords")
      ?.split(",")
      .map((k) => k.trim())
      .filter(Boolean) ?? [];

  const fields: Field[] = [
    {
      name: "Transaction",
      renderer: (metric) => {
        return (
          <Link href={`https://etherscan.io/tx/${metric.metadata.hash}`} target="_blank" className="text-inherit">
            {highlight(`${metric.metadata.hash.slice(0, 4)}...${metric.metadata.hash.slice(-4)}`, keywords)}
          </Link>
        );
      }
    },
    {
      name: "Direction",
      columnClassName: "text-center",
      cellClassName: "text-center",
      renderer: (metric) => (
        <Span
          className={clsx(
            "inline-flex mx-auto",
            "border border-current/30",
            "rounded-lg text-xs font-extrabold uppercase px-4 py-1 bg-current/10",
            metric.metadata.direction.toLowerCase() === "buy" ? "text-green" : "text-red"
          )}
        >
          {highlight(metric.metadata.direction, keywords)}
        </Span>
      )
    },
    {
      name: "Volume",
      columnClassName: "text-center",
      cellClassName: "text-center",
      renderer: (metric) => (
        <Span className={clsx("inline-flex mx-auto items-center font-semibold", metric.value.volume > 0 ? "text-green" : "text-red")}>
          {highlight(formatCurrency(metric.value.volume), keywords)}
        </Span>
      )
    },
    {
      name: "Gas",
      columnClassName: "text-center",
      cellClassName: "text-center",
      renderer: (metric) => <Span>{highlight(formatCurrency(metric.value.gas), keywords)}</Span>
    },
    {
      name: "Confirmed",
      columnClassName: "text-center",
      cellClassName: "text-center",
      renderer: (metric) => (
        <Span className="text-foreground-tertiary">{highlight(format.relativeTime(new Date(metric.timestamp), now), keywords)}</Span>
      )
    },
    {
      name: "Duration",
      columnClassName: "text-center",
      cellClassName: "text-center",
      renderer: (metric) => (
        <Span className="text-foreground-tertiary">{highlight(formatNumber(metric.value.duration, undefined, "ms"), keywords)}</Span>
      )
    }
  ];

  const transactions = useSWR(
    "platform/logistics/assets/activity/transactions",
    () => Spatial.metrics.read("transaction", undefined, undefined, undefined, "1m"),
    {
      refreshInterval: 10000,
      dedupingInterval: 15000
    }
  );

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

  const matchesKeywords = (metric: Metric, keys: string[]) => {
    if (keys.length === 0) {
      return true;
    }

    const haystack = [
      metric.metadata.hash,
      formatNumber(metric.value.duration),
      metric.metadata.direction,
      formatNumber(metric.value.price),
      formatNumber(metric.value.volume),
      formatNumber(metric.value.slippage * 100),
      formatNumber(metric.value.gas)
    ]
      .join(" ")
      .toLowerCase();

    return keys.some((k) => haystack.includes(k.toLowerCase()));
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

  const entries = (transactions.data && !transactions.data.error && transactions.data.data) || [];

  const PAGE_SIZE = 8;
  const PAGE = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  const filteredData = useMemo(() => {
    return entries.filter((metric) => matchesKeywords(metric, keywords));
  }, [entries, keywords]);

  const sortedData = useMemo(() => filteredData, [filteredData]);

  const paginatedData = useMemo(() => {
    const start = (PAGE - 1) * PAGE_SIZE;
    return sortedData.slice(start, start + PAGE_SIZE);
  }, [sortedData, PAGE]);

  const pages = Math.ceil(sortedData.length / PAGE_SIZE);

  useEffect(() => {
    setSearch(keywords.join(" "));
  }, [searchParams]);

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col w-screen xl:w-auto", props.className)}>
      <Container className="flex flex-col w-full xl:gap-10">
        <Container className="flex flex-col gap-10 px-10 xl:p-0 w-full">
          <H2 className="text-2xl font-bold">Activity</H2>
          <Container className="flex">
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
                placeholder="Search activity"
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
          </Container>
        </Container>
        <ScrollArea.Root className="w-full" fade fadeOrientation="horizontal">
          <ScrollArea.Viewport className="max-w-full px-10 xl:p-0">
            <Table.Root className="min-w-full table-fixed text-left border-separate border-spacing-y-10">
              <Table.Header className="text-foreground-quaternary">
                <Table.Row>
                  {fields.map((field, i) => (
                    <Table.Column
                      key={i}
                      className={clsx("font-semibold text-sm whitespace-nowrap px-10 first:px-0 last:pl-0 xl:last:px-0", field.columnClassName)}
                    >
                      {field.name}
                    </Table.Column>
                  ))}
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {(paginatedData ?? []).map((entry, i) => (
                  <Table.Row key={i}>
                    {fields.map((field, j) => (
                      <Table.Cell className={clsx("truncate px-10 first:px-0 last:pl-0 xl:last:px-0", field.cellClassName)} key={j}>
                        {field.renderer(entry)}
                      </Table.Cell>
                    ))}
                  </Table.Row>
                ))}
              </Table.Body>
            </Table.Root>
          </ScrollArea.Viewport>
          <ScrollArea.Scrollbar orientation="horizontal" className="opacity-0" />
        </ScrollArea.Root>
        <Pagination page={PAGE} pages={pages} className="self-center" onPageChange={navigate} />
      </Container>
    </Container>
  );
});

function formatCurrency(value?: number, currency: string = "USD") {
  if (value == null || isNaN(value)) {
    return "-";
  }

  if (value >= 1_000_000_000_000) {
    return formatNumber(value / 1_000_000_000_000, "T", currency);
  }

  if (value >= 1_000_000_000) {
    return formatNumber(value / 1_000_000_000, "B", currency);
  }

  if (value >= 1_000_000) {
    return formatNumber(value / 1_000_000, "M", currency);
  }

  if (value >= 1_000) {
    return formatNumber(value / 1_000, "K", currency);
  }

  return formatNumber(value, undefined, currency);
}

const formatNumber = (num: number, suffix?: string, unit?: string) => {
  const formatted = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(num);

  return `${formatted}${suffix ?? ""}${unit ? ` ${unit}` : ""}`;
};
