// Copyright © Spatial. All rights reserved.

"use client";

import {
  Line,
  LineChart as Primitive,
  XAxis,
  Tooltip,
  CartesianGrid,
  Legend,
} from "recharts";
import {
  ChartContainer,
  ChartLegend,
  ChartParameter,
  ChartTooltip,
  Element,
  LineChartProps,
  Node,
} from "../..";

/**
 * Create a new line chart element.
 * @param props Configurable options for the element.
 * @returns A line chart element.
 */
export const LineChart: Element<LineChartProps> = (
  props: LineChartProps,
): Node => {
  const parameter = (data: ChartParameter): Node => {
    return (
      <Line
        key={data.key}
        dataKey={data.key}
        type="natural"
        strokeWidth={2}
        stroke={data.color}
        name={data.name}
        dot={false}
        activeDot={{
          r: 3,
          fill: data.color,
          stroke: data.color,
        }}
      />
    );
  };

  return (
    <ChartContainer style={props.style} className={props.className}>
      <Primitive
        accessibilityLayer
        data={props.config.data}
        margin={{
          left: 12,
          right: 12,
        }}
      >
        <CartesianGrid stroke="var(--color-border-bounds)" vertical={false} />
        <Legend content={<ChartLegend />} />
        <Tooltip
          cursor={{ stroke: "var(--color-background-tertiary)" }}
          content={<ChartTooltip />}
        />
        <XAxis
          dataKey={props.axis}
          axisLine={false}
          tickLine={false}
          tickMargin={8}
          tickFormatter={props.axisFormatter}
        />
        {props.config.parameters.map(parameter)}
      </Primitive>
    </ChartContainer>
  );
};
