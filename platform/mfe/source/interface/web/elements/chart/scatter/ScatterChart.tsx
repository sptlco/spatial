// Copyright © Spatial. All rights reserved.

"use client";

import {
  CartesianGrid,
  ScatterChart as Primitive,
  Legend,
  Scatter,
  Tooltip,
  XAxis,
} from "recharts";
import {
  ChartContainer,
  ChartLegend,
  ChartParameter,
  ChartTooltip,
  Element,
  Node,
  ScatterChartProps,
} from "../..";

/**
 * Create a new scatter chart element.
 * @param props Configurable options for the element.
 * @returns A scatter chart element.
 */
export const ScatterChart: Element<ScatterChartProps> = (
  props: ScatterChartProps,
): Node => {
  const parameter = (data: ChartParameter): Node => {
    return (
      <Scatter
        key={data.key}
        dataKey={data.key}
        name={data.name}
        fill={data.color}
        color={data.color}
      />
    );
  };
  return (
    <ChartContainer style={props.style} className={props.className}>
      <Primitive
        data={props.config.data}
        accessibilityLayer
        margin={{
          left: 12,
          right: 12,
        }}
      >
        <CartesianGrid stroke="var(--color-border-bounds)" vertical={false} />
        <Legend content={<ChartLegend />} />
        <Tooltip
          cursor={{ stroke: "var(--color-background-tertiary)" }}
          content={
            <ChartTooltip
              force
              axis={props.axis}
              colors={props.config.parameters.map((param) => ({
                key: param.key,
                color: param.color,
              }))}
            />
          }
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
