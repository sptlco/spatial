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
   * The size of the icon, in pixels.
   */
  size?: number;

  /**
   * Whether or not to fill the icon.
   */
  fill?: boolean;
};

const styles = cva({
  base: "material-symbols-rounded",
  variants: {
    fill: {
      false: null,
      true: "fill"
    }
  }
});

/**
 * A small symbol that identifies an action or category.
 */
export const Icon = createElement<"span", IconProps>(({ fill = false, ...props }, ref) => (
  <Span {...props} ref={ref} className={clsx(styles({ fill }), props.className)} style={{ fontSize: props.size ?? 24, ...props.style }}>
    {props.symbol}
  </Span>
));
