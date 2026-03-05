"use client";

import { clsx } from "clsx";
import { cva } from "cva";
import { ReactNode } from "react";

import { Container, Icon, Link, Span, createElement } from "@sptlco/design";

type Severity = "none" | "warning" | "critical";

type Metric = {
  label: string;
  value: ReactNode;
  href: string;
  severity: Severity;
  status: string;
};

export const Metrics = createElement<typeof Container>((props, ref) => {
  const metrics: Metric[] = [
    { label: "Shipments", value: 128, href: "/platform/logistics/shipments", severity: "none", status: "Running" },
    { label: "Pending Orders", value: 42, href: "/platform/logistics/orders", severity: "warning", status: "Warnings" },
    {
      label: "Inventory Units",
      value: formatNumberSlim(12482),
      href: "/platform/logistics/inventory",
      severity: "none",
      status: "Running"
    },
    { label: "Wallet Balance", value: 7, href: "/platform/logistics/assets", severity: "critical", status: "Critical" }
  ];

  const severity = cva({
    variants: {
      severity: {
        none: "text-green",
        warning: "text-yellow",
        critical: "text-red"
      } as Record<Severity, any>
    }
  });

  return (
    <Container {...props} ref={ref} className="h-full grow grid grid-cols-2 gap-2.5 p-10 xl:p-0 sm:gap-10">
      {metrics.map((metric) => (
        <Link
          href={metric.href}
          key={metric.label}
          className={clsx(
            "group relative",
            "flex flex-col text-foreground-primary items-center justify-center",
            "p-5 aspect-square sm:aspect-auto xl:p-0 gap-2! xl:gap-5! bg-button xl:bg-button-ghost rounded-3xl xl:rounded-[56px] transition-all duration-200",
            "hover:bg-button-ghost-hover hover:text-white!",
            "active:bg-button-ghost-active active:text-white!"
          )}
        >
          <Span className="font-semibold text-xs xl:text-sm text-foreground-quaternary">{metric.label}</Span>
          <Span className="text-3xl xl:text-9xl font-extrabold">{metric.value}</Span>
          {metric.severity != "none" && (
            <Span className={clsx("inline-flex items-center gap-2", severity({ severity: metric.severity }))}>
              <Span className="border border-current rounded-lg text-xs font-extrabold uppercase px-4 py-1 bg-current/10">{metric.status}</Span>
            </Span>
          )}
          <Span className="absolute right-2.5 xl:right-10 bottom-2.5 xl:bottom-10">
            <Icon symbol="arrow_outward" className="xl:opacity-0 xl:group-hover:opacity-100 transition-all xl:text-3xl!" size={20} />
          </Span>
        </Link>
      ))}
    </Container>
  );
});

function formatNumberSlim(value?: number, unit?: string) {
  if (value == null || isNaN(value)) {
    return "-";
  }

  if (value >= 1_000_000_000_000) {
    return formatNumber(value / 1_000_000_000_000, "T", unit);
  }

  if (value >= 1_000_000_000) {
    return formatNumber(value / 1_000_000_000, "B", unit);
  }

  if (value >= 1_000_000) {
    return formatNumber(value / 1_000_000, "M", unit);
  }

  if (value >= 1_000) {
    return formatNumber(value / 1_000, "K", unit);
  }

  return formatNumber(value, undefined, unit);
}

const formatNumber = (num: number, suffix?: string, unit?: string) => {
  const formatted = new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(num);

  return `${formatted}${suffix ?? ""}${unit ? ` ${unit}` : ""}`;
};
