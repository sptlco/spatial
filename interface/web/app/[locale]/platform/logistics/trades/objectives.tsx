// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { ReactNode } from "react";

import { Container, createElement, H2, Icon, Span, Tooltip } from "@sptlco/design";

type Objective = {
  label: string;
  tip?: string;
  value: ReactNode;
  diff?: number;
};

/**
 * An element that displays the current trading objectives.
 */
export const Objectives = createElement<typeof Container>((props, ref) => {
  const objectives: Objective[] = [
    { label: "Trades", value: formatNumber(54), diff: 0.0604 },
    { label: "Lots", value: formatNumber(286), diff: -0.042 },
    { label: "Average RRR", value: formatNumber(0.0), tip: "Testing 1, 2" },
    { label: "Win Rate", value: formatNumber(65.39) + "%" },
    { label: "Trading Days", value: formatNumber(15), diff: 0.042 },
    { label: "Live Profit Share", value: formatNumber(80) + "%" },
    { label: "Avg. Winning Trade", value: formatCurrency(458.06) },
    { label: "Avg. Losing Trade", value: formatCurrency(-1021.06) }
  ];

  return (
    <Container {...props} ref={ref} className={clsx("flex flex-col gap-6", props.className)}>
      <H2 className="px-10 text-2xl font-bold">Objectives</H2>
      <Container className="flex w-full rounded-4xl bg-background-surface">
        {objectives.map((objective, i) => (
          <Container key={i} className="grow flex flex-col gap-4 p-10 whitespace-nowrap">
            <Span className="text-sm text-foreground-quaternary font-semibold flex items-center gap-2">
              <Span>{objective.label}</Span>
              {objective.tip && (
                <Tooltip.Root>
                  <Tooltip.Trigger className="flex cursor-pointer">
                    <Icon symbol="info" className="font-semibold" size={20} fill />
                  </Tooltip.Trigger>
                  <Tooltip.Content sideOffset={20}>{objective.tip}</Tooltip.Content>
                </Tooltip.Root>
              )}
            </Span>
            <Span className="text-4xl flex items-center gap-4 whitespace-nowrap">
              <Span>{objective.value}</Span>
              {objective.diff && (
                <Span className={clsx("text-xl", objective.diff > 0 ? "text-green" : "text-red")}>
                  {objective.diff > 0 ? "+" : "-"}
                  {(objective.diff * 100).toFixed(2)}%
                </Span>
              )}
            </Span>
          </Container>
        ))}
      </Container>
    </Container>
  );
});

function formatCurrency(value?: number) {
  if (value == null) return "-";

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    currencyDisplay: "code",
    currencySign: "accounting",
    maximumFractionDigits: 2
  }).format(value);
}

function formatNumber(value?: number) {
  if (value == null) return "-";
  return new Intl.NumberFormat("en-US").format(value);
}
