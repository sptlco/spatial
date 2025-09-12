// Copyright © Spatial Corporation. All rights reserved.

"use client";

import {
  Area,
  CartesianGrid,
  AreaChart as Primitive,
  Legend,
  Tooltip,
  XAxis,
} from "recharts";
import {
  AreaChartProps,
  ChartContainer,
  ChartLegend,
  ChartParameter,
  ChartTooltip,
  Element,
  Node,
} from "../..";

/**
 * Create a new area chart element.
 * @param props Configurable options for the element.
 * @returns An area chart element.
 */
export const AreaChart: Element<AreaChartProps> = (
  props: AreaChartProps,
): Node => {
  const parameter = (data: ChartParameter): Node => {
    return (
      <Area
        key={data.key}
        dataKey={data.key}
        name={data.name}
        type="natural"
        fill={data.color}
        fillOpacity={0.6}
        stroke={data.color}
        stackId={props.stack ? props.axis : undefined}
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
