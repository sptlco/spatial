// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Metric } from "@sptlco/data";
import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { useFormatter, useNow } from "next-intl";
import { Fragment, ReactNode, useEffect, useMemo, useState } from "react";
import useSWR from "swr";

import {
  Button,
  Combobox,
  Container,
  createElement,
  Dialog,
  Field,
  Form,
  H2,
  Icon,
  Link,
  Pagination,
  ScrollArea,
  Span,
  Spinner,
  Table
} from "@sptlco/design";

type FieldDef = {
  name: string;
  renderer: (metric: Metric) => ReactNode;
  columnClassName?: string;
  cellClassName?: string;
};

type SortField = "timestamp" | "direction" | "volume" | "gas" | "duration";
type SortOrder = `${SortField}-${"asc" | "desc"}` | "";
type Direction = "buy" | "sell";

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

const Search = createElement<typeof Fragment>(() => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState(() => searchParams.get("keywords") ?? "");

  const commit = (value: string) => {
    const params = new URLSearchParams(searchParams.toString());
    const next = value.trim();

    if (next) {
      params.set("keywords", next);
    } else {
      params.delete("keywords");
    }

    params.delete("page");
    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
    setOpen(false);
  };

  const onOpenChange = (next: boolean) => {
    if (next) setSearch(searchParams.get("keywords") ?? "");
    setOpen(next);
  };

  const active = !!searchParams.get("keywords");

  return (
    <Dialog.Root open={open} onOpenChange={onOpenChange}>
      <Dialog.Trigger asChild>
        <Button intent="none" size="fit" className="p-2">
          <Icon symbol="search" className={active ? "fill" : undefined} />
        </Button>
      </Dialog.Trigger>
      <Dialog.Content className="p-2.5! bg-transparent!" closeButton={false}>
        <Form
          className="group flex relative w-full max-w-md gap-4 items-end"
          onSubmit={(e) => {
            e.preventDefault();
            commit(search);
          }}
        >
          <Icon symbol="search" size={20} className="absolute left-2.5 bottom-2.5" />
          <Field
            type="text"
            placeholder="Search transactions"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Escape") {
                e.preventDefault();
                commit("");
                setSearch("");
              }
            }}
            className="pl-10 pr-12"
            autoFocus
          />
          {search ? (
            <Button
              type="button"
              className="shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5"
              onClick={() => {
                commit("");
                setSearch("");
              }}
            >
              <Icon symbol="close" size={20} />
            </Button>
          ) : (
            <Button type="submit" className="shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5 group-focus-within:bg-button-highlight!">
              <Icon symbol="arrow_right_alt" size={20} />
            </Button>
          )}
        </Form>
      </Dialog.Content>
    </Dialog.Root>
  );
});

const DIRECTIONS: Direction[] = ["buy", "sell"];

const Filters = createElement<typeof Fragment, { selection?: Direction[]; onSelectionChange?: (selection: Direction[]) => void }>((props, _) => {
  const selection = props.selection ?? [];

  const toggle = (value: string) => {
    const dir = value as Direction;
    const next = selection.includes(dir) ? selection.filter((d) => d !== dir) : [...selection, dir];
    props.onSelectionChange?.(next);
  };

  const active = selection.length > 0;

  return (
    <Combobox.Root multiple selection={selection} onSelect={toggle}>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="filter_alt" className={clsx(active ? "fill" : "group-data-[state=open]:fill")} />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List label="Direction">
          {DIRECTIONS.map((dir) => (
            <Combobox.Item key={dir} value={dir} label={dir.charAt(0).toUpperCase() + dir.slice(1)} />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
});

const SORT_FIELDS: Record<SortField, { name: string; icon: string }> = {
  timestamp: { name: "Confirmed", icon: "calendar_today" },
  direction: { name: "Direction", icon: "swap_vert" },
  volume: { name: "Volume", icon: "paid" },
  gas: { name: "Gas", icon: "local_gas_station" },
  duration: { name: "Duration", icon: "timer" }
};

const Sort = createElement<typeof Fragment, { selection?: SortOrder; onSelectionChange?: (selection: SortOrder) => void }>((props, _) => {
  const selection = props.selection ?? "";

  const toggle = (field: string) => {
    const asc = `${field}-asc` as SortOrder;
    const desc = `${field}-desc` as SortOrder;

    let next: SortOrder;
    if (selection === asc) next = desc;
    else if (selection === desc) next = "";
    else next = asc;

    props.onSelectionChange?.(next);
  };

  const active = selection !== "";
  const activeField = active ? (selection.replace(/-.*/, "") as SortField) : null;

  const direction = selection.endsWith("asc") ? (
    <Icon symbol="sort" size={16} className="-scale-x-100 rotate-90" />
  ) : selection.endsWith("desc") ? (
    <Icon symbol="sort" size={16} className="-rotate-90" />
  ) : null;

  return (
    <Combobox.Root multiple selection={activeField ? [activeField] : []} onSelect={toggle}>
      <Combobox.Trigger asChild>
        <Button intent="ghost" size="fit" className="group p-2 data-[state=open]:bg-button-ghost-active">
          <Icon symbol="sort" className={clsx(active ? "fill" : "group-data-[state=open]:fill")} />
        </Button>
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List label="Sort by">
          {(Object.entries(SORT_FIELDS) as [SortField, { name: string; icon: string }][]).map(([key, field]) => (
            <Combobox.Item
              key={key}
              value={key}
              label={field.name}
              indicator={activeField === key ? <Span className="text-hint">{direction}</Span> : undefined}
            />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
});

/**
 * An element that displays the current trading objectives.
 */
export const Activity = createElement<typeof Container>((props, ref) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();
  const now = useNow();
  const format = useFormatter();

  const [directionFilters, setDirectionFilters] = useState<Direction[]>([]);
  const [sort, setSort] = useState<SortOrder>("");

  const keywords = useMemo(
    () =>
      searchParams
        .get("keywords")
        ?.split(",")
        .map((k) => k.trim())
        .filter(Boolean) ?? [],
    [searchParams]
  );

  const fields: FieldDef[] = [
    {
      name: "Transaction",
      renderer: (metric) => (
        <Link href={`https://etherscan.io/tx/${metric.metadata.hash}`} target="_blank" className="text-inherit">
          {highlight(`${metric.metadata.hash.slice(0, 4)}...${metric.metadata.hash.slice(-4)}`, keywords)}
        </Link>
      )
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

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("page", page.toString());
    } else {
      params.delete("page");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const matchesKeywords = (metric: Metric, keys: string[]) => {
    if (keys.length === 0) return true;

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

  const entries = transactions.data ?? [];

  const PAGE_SIZE = 8;
  const PAGE = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  const filteredData = useMemo(() => {
    return entries.filter((metric) => {
      const matchesDir = directionFilters.length === 0 || directionFilters.includes(metric.metadata.direction.toLowerCase() as Direction);

      return matchesDir && matchesKeywords(metric, keywords);
    });
  }, [entries, keywords, directionFilters]);

  const sortedData = useMemo(() => {
    if (!sort) return filteredData;

    const [field, dir] = sort.split("-") as [SortField, "asc" | "desc"];
    const mul = dir === "asc" ? 1 : -1;

    return [...filteredData].sort((a, b) => {
      switch (field) {
        case "timestamp":
          return (new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime()) * mul;
        case "direction":
          return a.metadata.direction.localeCompare(b.metadata.direction) * mul;
        case "volume":
          return (a.value.volume - b.value.volume) * mul;
        case "gas":
          return (a.value.gas - b.value.gas) * mul;
        case "duration":
          return (a.value.duration - b.value.duration) * mul;
      }
    });
  }, [filteredData, sort]);

  const paginatedData = useMemo(() => {
    const start = (PAGE - 1) * PAGE_SIZE;
    return sortedData.slice(start, start + PAGE_SIZE);
  }, [sortedData, PAGE]);

  const pages = Math.ceil(sortedData.length / PAGE_SIZE);

  useEffect(() => {
    navigate(1);
  }, [directionFilters, sort, searchParams.get("keywords")]);

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col w-screen xl:w-auto", props.className)}>
      <Container className="flex flex-col w-full xl:gap-10">
        <Container className="flex flex-col gap-4 px-10 pt-10 xl:p-0 w-full">
          <Container className="flex flex-col xl:flex-row items-center gap-4">
            <Container className="flex items-center gap-3">
              <H2 className="text-2xl font-bold">Transactions</H2>
              <Span className="bg-translucent shrink-0 size-10 font-normal rounded-full text-sm inline-flex items-center justify-center">
                {transactions.isValidating || !transactions.data ? <Spinner className="size-3 text-foreground-secondary" /> : sortedData.length}
              </Span>
            </Container>
            <Container className="flex items-center">
              <Search />
              <Filters selection={directionFilters} onSelectionChange={setDirectionFilters} />
              <Sort selection={sort} onSelectionChange={setSort} />
            </Container>
          </Container>

          {(directionFilters.length > 0 || sort) && (
            <Container className="flex flex-wrap gap-2">
              {directionFilters.map((dir) => (
                <Button
                  key={dir}
                  intent="ghost"
                  size="fit"
                  className="flex items-center gap-1.5 px-3 py-1 text-xs font-semibold rounded-full border border-current/20"
                  onClick={() => setDirectionFilters((prev) => prev.filter((d) => d !== dir))}
                >
                  {dir.charAt(0).toUpperCase() + dir.slice(1)}
                  <Icon symbol="close" size={14} />
                </Button>
              ))}
              {sort && (
                <Button
                  intent="ghost"
                  size="fit"
                  className="flex items-center gap-1.5 px-3 py-1 text-xs font-semibold rounded-full border border-current/20"
                  onClick={() => setSort("")}
                >
                  {SORT_FIELDS[sort.replace(/-.*/, "") as SortField].name}
                  {sort.endsWith("asc") ? (
                    <Icon symbol="sort" size={14} className="-scale-x-100 rotate-90" />
                  ) : (
                    <Icon symbol="sort" size={14} className="-rotate-90" />
                  )}
                  <Icon symbol="close" size={14} />
                </Button>
              )}
            </Container>
          )}
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
                {paginatedData.map((entry, i) => (
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
  if (value == null || isNaN(value)) return "-";

  if (value >= 1_000_000_000_000) return formatNumber(value / 1_000_000_000_000, "T", currency);
  if (value >= 1_000_000_000) return formatNumber(value / 1_000_000_000, "B", currency);
  if (value >= 1_000_000) return formatNumber(value / 1_000_000, "M", currency);
  if (value >= 1_000) return formatNumber(value / 1_000, "K", currency);

  return formatNumber(value, undefined, currency);
}

const formatNumber = (num: number, suffix?: string, unit?: string) => {
  const formatted = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(num);

  return `${formatted}${suffix ?? ""}${unit ? ` ${unit}` : ""}`;
};
