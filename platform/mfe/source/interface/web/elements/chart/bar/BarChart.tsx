// Copyright © Spatial. All rights reserved.

"use client";

import {
  BarChart as Primitive,
  Bar,
  CartesianGrid,
  Tooltip,
  XAxis,
  Legend,
} from "recharts";
import {
  BarChartProps,
  ChartContainer,
  ChartLegend,
  ChartParameter,
  ChartTooltip,
  Element,
  Node,
} from "../..";

/**
 * Create a new bar chart element.
 * @param props Configurable options for the element.
 * @returns A bar chart element.
 */
export const BarChart: Element<BarChartProps> = (
  props: BarChartProps,
): Node => {
  const parameter = (data: ChartParameter): Node => {
    return (
      <Bar
        key={data.key}
        dataKey={data.key}
        fill={data.color}
        name={data.name}
      />
    );
  };

  return (
    <ChartContainer style={props.style} className={props.className}>
      <Primitive
        accessibilityLayer
        data={props.config.data}
        maxBarSize={16}
        margin={{
          left: 12,
          right: 12,
        }}
      >
        <CartesianGrid stroke="var(--color-border-bounds)" vertical={false} />
        <Legend content={<ChartLegend />} />
        <Tooltip
          cursor={{ fill: "var(--color-background-tertiary)" }}
          content={<ChartTooltip />}
        />
        <XAxis
          dataKey={props.axis}
          axisLine={false}
          tickLine={false}
          tickMargin={8}
          tickFormatter={props.axisFormatter}
        />
        {props.config.parameters.map((param) => parameter(param))}
      </Primitive>
    </ChartContainer>
  );
};
