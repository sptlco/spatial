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
        const hash = `${metric.metadata.hash.slice(0, 6)}...${metric.metadata.hash.slice(-4)}`;
        return (
          <Link href={`https://etherscan.io/address/${metric.metadata.hash}`} target="_blank" className="text-inherit">
            {highlight(hash, keywords)}
          </Link>
        );
      }
    },
    {
      name: "Duration",
      renderer: (metric) => <Span className="text-foreground-tertiary">{highlight(`${metric.value.duration.toFixed(2)} ms`, keywords)}</Span>
    },
    {
      name: "Confirmed",
      renderer: (metric) => (
        <Span className="text-foreground-tertiary">{highlight(format.relativeTime(new Date(metric.timestamp), now), keywords)}</Span>
      )
    },
    {
      name: "Direction",
      renderer: (metric) => (
        <Span
          className={clsx(
            "border border-current/30",
            "rounded-lg text-xs font-extrabold uppercase px-4 py-1 bg-current/10",
            metric.metadata.direction.toLowerCase() === "buy" ? "text-red" : "text-green"
          )}
        >
          {highlight(metric.metadata.direction, keywords)}
        </Span>
      )
    },
    {
      name: "Price",
      renderer: (metric) => {
        const formatted = formatCurrency(1 + 1 * metric.value.deviation);
        return <Span>{highlight(formatted, keywords)}</Span>;
      }
    },
    {
      name: "Deviation",
      renderer: (metric) => (
        <Span className={clsx("flex items-center", metric.value.deviation > 0 ? "text-green" : "text-red")}>
          <Span className="inline-flex items-center justify-center">
            {metric.value.deviation > 0 ? <Icon symbol="arrow_drop_up" size={32} /> : <Icon symbol="arrow_drop_down" size={32} />}
          </Span>
          <Span className="font-semibold text-sm">{highlight(`${Math.abs(metric.value.deviation * 100).toFixed(2)}%`, keywords)}</Span>
        </Span>
      )
    },
    {
      name: "Volume",
      renderer: (metric) => {
        const formatted = formatCurrency(metric.value.volume);
        return <Span>{highlight(formatted, keywords)}</Span>;
      }
    },
    {
      name: "Slippage",
      renderer: (metric) => {
        const formatted = `${(metric.value.slippage * 100).toFixed(2)}%`;
        return <Span>{highlight(formatted, keywords)}</Span>;
      }
    },
    {
      name: "Gas",
      renderer: (metric) => {
        const formatted = formatCurrency(metric.value.gas);
        return <Span>{highlight(formatted, keywords)}</Span>;
      }
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
      metric.value.duration.toFixed(2),
      metric.metadata.direction,
      metric.value.price.toFixed(2),
      (metric.value.deviation * 100).toFixed(2),
      metric.value.volume.toFixed(2),
      (metric.value.slippage * 100).toFixed(2),
      metric.value.gas.toFixed(2)
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
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-10 w-screen xl:w-auto", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Activity</H2>
      <Container className="flex flex-col w-full">
        <Container className="flex px-10">
          <Form
            className="relative w-full max-w-sm flex items-center"
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
              <Button type="button" intent="ghost" className="absolute right-1 size-8! p-0!" onClick={clearKeywords}>
                <Icon symbol="close" />
              </Button>
            ) : (
              <Button type="submit" intent="ghost" className="absolute right-1 size-8! p-0!">
                <Icon symbol="arrow_forward" />
              </Button>
            )}
          </Form>
        </Container>
        <ScrollArea.Root className="w-full" fade fadeOrientation="horizontal">
          <ScrollArea.Viewport className="max-w-full">
            <Table.Root className="min-w-full table-fixed text-left border-separate border-spacing-10">
              <Table.Header className="text-foreground-quaternary">
                <Table.Row>
                  {fields.map((metric, i) => (
                    <Table.Column key={i} className="font-semibold text-sm whitespace-nowrap">
                      {metric.name}
                    </Table.Column>
                  ))}
                </Table.Row>
              </Table.Header>
              <Table.Body>
                {(paginatedData ?? []).map((entry, i) => (
                  <Table.Row key={i}>
                    {fields.map((metric, j) => (
                      <Table.Cell className="truncate" key={j}>
                        {metric.renderer(entry)}
                      </Table.Cell>
                    ))}
                  </Table.Row>
                ))}
              </Table.Body>
            </Table.Root>
          </ScrollArea.Viewport>
          <ScrollArea.Scrollbar orientation="horizontal" />
        </ScrollArea.Root>
        <Pagination page={PAGE} pages={pages} className="self-center" onPageChange={navigate} />
      </Container>
    </Container>
  );
});

function formatCurrency(value?: number) {
  if (value == null || isNaN(value)) {
    return "-";
  }

  const abs = Math.abs(value);
  const format = (num: number, suffix = "") => {
    const formatted = new Intl.NumberFormat("en-US", {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2
    }).format(num);

    return `${value < 0 ? "-" : ""}${formatted}${suffix} USD`;
  };

  if (abs >= 1_000_000_000_000) {
    return format(abs / 1_000_000_000_000, "T");
  }

  if (abs >= 1_000_000_000) {
    return format(abs / 1_000_000_000, "B");
  }

  if (abs >= 1_000_000) {
    return format(abs / 1_000_000, "M");
  }

  if (abs >= 1_000) {
    return format(abs / 1_000, "K");
  }

  return `${new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(value)} USD`;
}
