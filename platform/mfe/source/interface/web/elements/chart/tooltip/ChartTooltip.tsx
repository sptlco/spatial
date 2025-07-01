// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { ChartTooltipProps, Element, LI, Node, Span, UL } from "../..";

/**
 * Create a new chart tooltip element.
 * @param props Configurable options for the element.
 * @returns A chart tooltip element.
 */
export const ChartTooltip: Element<ChartTooltipProps> = (
  props: ChartTooltipProps,
): Node => {
  if (!props.active || !props.payload || props.payload.length <= 0) {
    return null;
  }

  const parameter = (payload: any, key: any): Node => {
    return (
      <LI className="gap-1/2u flex items-center" key={key}>
        <Span
          className="size-1/2u flex rounded-[2px]"
          style={{
            backgroundColor:
              payload.color ||
              payload.payload.fill ||
              props.colors?.find((c) => c.key === payload.dataKey)?.color,
          }}
        />
        <Span>{payload.name}</Span>
        <Span className="text-foreground-secondary">{payload.value}</Span>
      </LI>
    );
  };

  return (
    <UL className="outline-border-bounds rounded-1/2u py-1/2u px-3/4u text-s bg-background-primary flex flex-col outline outline-1">
      {!props.hideLabel && (
        <Span className="font-bold">
          {props.label ||
            (props.force &&
              props.payload.find((p) => p.dataKey == props.axis).value)}
        </Span>
      )}
      {props.payload
        .filter(
          (p) => !props.force || props.colors?.find((c) => c.key === p.dataKey),
        )
        .map((payload, i) => parameter(payload, i))}
    </UL>
  );
};
