"use client";

import { clsx } from "clsx";

import { Container, Icon, Link, Span, createElement } from "@sptlco/design";

type Metric = {
  label: string;
  value: number;
  href: string;
};

export const Metrics = createElement<typeof Container>((props, ref) => {
  const metrics: Metric[] = [
    { label: "Active Shipments", value: 128, href: "/platform/shipments" },
    { label: "Pending Orders", value: 42, href: "/platform/orders" },
    { label: "Inventory Units", value: 12482, href: "/platform/inventory" },
    { label: "Active Trades", value: 7, href: "/platform/trades" }
  ];

  return (
    <Container {...props} ref={ref} className="grid grid-cols-1 xl:grid-cols-4 gap-10">
      {metrics.map((metric) => (
        <Link
          href={metric.href}
          key={metric.label}
          className={clsx(
            "group relative",
            "flex flex-col text-foreground-primary items-start justify-center",
            "p-10 gap-2! bg-background-subtle xl:rounded-4xl transition-all duration-200",
            "hover:bg-button-highlight-hover hover:text-white!",
            "active:bg-button-highlight-active active:text-white!"
          )}
        >
          <Span className="text-hint text-xs font-extrabold uppercase">{metric.label}</Span>
          <Span className="text-3xl font-extrabold">{metric.value}</Span>
          <Span
            className={clsx(
              "absolute right-10 xl:opacity-0",
              "group-hover:opacity-100 group-hover:animate-in text-hint xl:text-white group-hover:fade-in group-hover:slide-in-from-right-2"
            )}
          >
            <Icon symbol="arrow_right_alt" size={32} />
          </Span>
        </Link>
      ))}
    </Container>
  );
});
