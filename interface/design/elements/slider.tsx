// Copyright © Spatial Corporation. All rights reserved.

"use client";

import * as Primitive from "@radix-ui/react-slider";
import { clsx } from "clsx";
import { useMemo } from "react";

import { createElement } from "..";

/**
 * An input where the user selects a value from within a given range.
 */
export const Slider = createElement<typeof Primitive.Root>(({ className, defaultValue, value, min = 0, max = 100, ...props }, ref) => {
  const _values = useMemo(
    () => (Array.isArray(value) ? value : Array.isArray(defaultValue) ? defaultValue : [min, max]),
    [value, defaultValue, min, max]
  );

  return (
    <Primitive.Root
      {...props}
      ref={ref}
      data-slot="slider"
      defaultValue={defaultValue}
      value={value}
      min={min}
      max={max}
      className={clsx(
        "relative flex w-full",
        "data-[orientation=vertical]:h-full data-[orientation=vertical]:w-auto data-[orientation=vertical]:flex-col data-[orientation=vertical]:min-h-40",
        "touch-none items-center select-none data-disabled:opacity-50",
        className
      )}
    >
      <Primitive.Track
        data-slot="slider-track"
        className="bg-input rounded-full data-[orientation=horizontal]:h-1 data-[orientation=vertical]:w-1 relative grow overflow-hidden data-[orientation=horizontal]:w-full data-[orientation=vertical]:h-full"
      >
        <Primitive.Range
          data-slot="slider-range"
          className="bg-foreground-accent absolute select-none data-[orientation=horizontal]:h-full data-[orientation=vertical]:w-full"
        />
      </Primitive.Track>
      {Array.from({ length: _values.length }, (_, index) => (
        <Primitive.Thumb
          key={index}
          data-slot="slider-thumb"
          className="border-foreground-accent ring-foreground-accent/50 relative size-3 rounded-full border bg-foreground-accent transition-[color,box-shadow] after:absolute after:-inset-2 hover:ring-3 focus-visible:ring-3 focus-visible:outline-hidden active:ring-3 block shrink-0 select-none disabled:pointer-events-none disabled:opacity-50"
        />
      ))}
    </Primitive.Root>
  );
});
