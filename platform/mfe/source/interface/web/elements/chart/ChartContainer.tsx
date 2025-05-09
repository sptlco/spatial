// Copyright © Spatial. All rights reserved.

"use client";

import clsx from "clsx";
import { Element, ElementProps, Node } from "..";
import { ResponsiveContainer } from "recharts";
import { ReactElement } from "react";

/**
 * Create a new chart container element.
 * @param props Configurable options for the element.
 * @returns A chart container element.
 */
export const ChartContainer: Element = (props: ElementProps): Node => {
  return (
    <ResponsiveContainer
      style={props.style}
      className={clsx("max-h-16u aspect-square w-full", props.className)}
      children={props.children as ReactElement}
    />
  );
};
