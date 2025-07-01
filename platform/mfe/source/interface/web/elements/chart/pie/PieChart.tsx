// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { PieChart as Primitive, Legend, Pie, Tooltip } from "recharts";
import {
  ChartContainer,
  ChartLegend,
  ChartTooltip,
  Element,
  Node,
  PieChartProps,
} from "../..";

/**
 * Create a new pie chart element.
 * @param props Configurable options for the element.
 * @returns A pie chart element.
 */
export const PieChart: Element<PieChartProps> = (
  props: PieChartProps,
): Node => {
  return (
    <ChartContainer style={props.style} className={props.className}>
      <Primitive accessibilityLayer>
        <Pie
          stroke="none"
          dataKey="value"
          nameKey="name"
          data={props.config.data?.map((entry) => ({
            name: entry.name,
            value: entry.value,
            fill: entry.color,
          }))}
        />
        <Tooltip content={<ChartTooltip />} />
        <Legend content={<ChartLegend />} />
      </Primitive>
    </ChartContainer>
  );
};
