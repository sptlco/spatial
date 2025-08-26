// Copyright © Spatial Corporation. All rights reserved.

"use client";

import {
  RadialBarChart as Primitive,
  Label,
  PolarGrid,
  PolarRadiusAxis,
  RadialBar,
  Tooltip,
} from "recharts";

import {
  ChartContainer,
  ChartTooltip,
  Element,
  Node,
  RadialChartProps,
} from "../..";

/**
 * Create a new radial chart element.
 * @param props Configurable options for the element.
 * @returns A radial chart element.
 */
export const RadialChart: Element<RadialChartProps> = ({
  value = 0,
  ...props
}: RadialChartProps): Node => {
  const { name, max, color, label, formatter } = props.config;

  return (
    <ChartContainer style={props.style} className={props.className}>
      <Primitive
        accessibilityLayer
        data={[{ value, fill: color }]}
        innerRadius={80}
        outerRadius={100}
        startAngle={0}
        endAngle={(value / (max || value)) * 360}
      >
        <PolarGrid
          gridType="circle"
          radialLines={false}
          stroke="none"
          className="first:fill-background-tertiary last:fill-background-primary"
          polarRadius={[84, 76]}
        />
        <RadialBar dataKey="value" name={name} fill={color} cornerRadius={10} />
        <PolarRadiusAxis tick={false} tickLine={false} axisLine={false}>
          <Label
            content={({ viewBox }) => {
              if (viewBox && "cx" in viewBox && "cy" in viewBox) {
                return (
                  <text
                    x={viewBox.cx}
                    y={viewBox.cy}
                    textAnchor="middle"
                    dominantBaseline="middle"
                  >
                    <tspan
                      x={viewBox.cx}
                      y={viewBox.cy}
                      className="fill-foreground-primary text-3xl font-bold"
                    >
                      {formatter ? formatter(value, max || value) : value}
                    </tspan>
                    {label && (
                      <tspan
                        x={viewBox.cx}
                        y={(viewBox.cy || 0) + 24}
                        className="fill-foreground-tertiary"
                      >
                        {label}
                      </tspan>
                    )}
                  </text>
                );
              }
            }}
          />
        </PolarRadiusAxis>
        <Tooltip cursor={false} content={<ChartTooltip hideLabel />} />
      </Primitive>
    </ChartContainer>
  );
};
