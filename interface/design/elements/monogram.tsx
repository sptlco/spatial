// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Svg } from "..";
import { clsx } from "clsx";

export const Monogram = createElement<typeof Svg, { text: string }>((props, ref) => {
  const words = props.text.trim().split(/\s+/).filter(Boolean);
  const initials = words.length >= 2 ? `${words[0][0]}${words[words.length - 1][0]}` : words.length === 1 ? words[0][0] : "?";

  const contrast = (color: string): "black" | "white" => {
    const rgb = color
      .replace("#", "")
      .match(/.{2}/g)!
      .map((c) => parseInt(c, 16) / 255)
      .map((c) => (c <= 0.03928 ? c / 12.92 : Math.pow((c + 0.055) / 1.055, 2.4)));

    return 0.2126 * rgb[0] + 0.7152 * rgb[1] + 0.0722 * rgb[2] > 0.179 ? "black" : "white";
  };

  return (
    <Svg
      {...props}
      ref={ref}
      viewBox="0 0 100 100"
      role="img"
      aria-label={props.text}
      className={clsx("rounded-full aspect-square overflow-hidden", props.className)}
    >
      <circle cx="50" cy="50" r="50" fill="currentColor" />
      <text
        x="50"
        y="50"
        textAnchor="middle"
        dominantBaseline="central"
        fontSize="40"
        fontWeight="600"
        fill={contrast(props.style?.color || "#FFFFFF")}
      >
        {initials.toUpperCase()}
      </text>
    </Svg>
  );
});
