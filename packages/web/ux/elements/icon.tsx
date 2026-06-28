// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { cva } from "cva";

import { createElement, Span } from "..";

/**
 * Configurable options for an icon.
 */
export type IconProps = {
  /**
   * The symbol to render.
   */
  symbol: string;

  /**
   * The icon's {@link IconVariant}.
   */
  variant?: IconVariant;

  /**
   * The size of the icon, in pixels.
   */
  size?: number;

  /**
   * The icon's stroke weight.
   */
  weight?: IconWeight;
};

/**
 * The style of an icon.
 */
export type IconVariant = "rounded" | "sharp";

/**
 * The stroke weight of an icon.
 */
export type IconWeight = 100 | 200 | 300 | 400 | 500 | 600 | 700;

const icon = cva({
  variants: {
    variant: {
      rounded: "material-symbols-rounded",
      sharp: "material-symbols-sharp"
    }
  }
});

/**
 * A small symbol that identifies an action or category.
 */
export const Icon = createElement<"span", IconProps>(({ variant = "rounded", ...props }, ref) => (
  <Span
    {...props}
    ref={ref}
    className={clsx(icon({ variant }), props.className)}
    style={{
      fontSize: props.size ?? 24,
      fontWeight: props.weight ?? 200,
      ...props.style
    }}
  >
    {props.symbol}
  </Span>
));
