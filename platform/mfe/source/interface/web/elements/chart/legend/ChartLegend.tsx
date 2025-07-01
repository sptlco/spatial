// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { ChartLegendProps, Element, LI, Node, Span, UL } from "../..";

/**
 * Create a new chart legend element.
 * @param props Configurable options for the element.
 * @returns A chart legend element.
 */
export const ChartLegend: Element<ChartLegendProps> = (
  props: ChartLegendProps,
): Node => {
  return (
    <UL className="gap-3/4u py-1/4u flex items-center justify-center">
      {props.payload?.map((param: any, i) => (
        <LI key={i} className="gap-1/2u flex items-center">
          <Span
            className="size-1/2u flex rounded-[2px]"
            style={{ backgroundColor: param.color }}
          />
          <Span>{param.value}</Span>
        </LI>
      ))}
    </UL>
  );
};
