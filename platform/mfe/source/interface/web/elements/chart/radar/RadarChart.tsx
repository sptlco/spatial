// Copyright © Spatial. All rights reserved.

"use client";

import {
  RadarChart as Primitive,
  Legend,
  PolarAngleAxis,
  PolarGrid,
  Radar,
  Tooltip,
} from "recharts";
import {
  ChartContainer,
  ChartLegend,
  ChartParameter,
  ChartTooltip,
  Element,
  Node,
  RadarChartProps,
} from "../..";

/**
 * Create a new radar chart element.
 * @param props Configurable options for the element.
 * @returns A radar chart element.
 */
export const RadarChart: Element<RadarChartProps> = (
  props: RadarChartProps,
): Node => {
  const parameter = (data: ChartParameter): Node => {
    return (
      <Radar
        key={data.key}
        name={data.name}
        dataKey={data.key}
        fill={data.color}
        fillOpacity={0.6}
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
      <Primitive accessibilityLayer data={props.config.data}>
        <PolarGrid gridType="circle" stroke="var(--color-border-bounds)" />
        <Legend content={<ChartLegend />} />
        <Tooltip
          cursor={{ stroke: "var(--color-foreground-primary" }}
          content={<ChartTooltip />}
        />
        <PolarAngleAxis dataKey={props.axis} />
        {props.config.parameters.map(parameter)}
      </Primitive>
    </ChartContainer>
  );
};
