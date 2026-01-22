// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Image, ImageProps } from "..";
import { clsx } from "clsx";

/**
 * An image element representing the user.
 */
export const Avatar = createElement<"img", ImageProps & { src?: string }>((props, ref) => {
  const { src, alt = "", className, ...rest } = props;

  if (!src) {
    const words = alt.trim().split(/\s+/).filter(Boolean);
    const initials = words.length >= 2 ? `${words[0][0]}${words[words.length - 1][0]}` : words.length === 1 ? words[0][0] : "?";

    return (
      <svg viewBox="0 0 100 100" role="img" aria-label={alt} className={clsx("rounded-full aspect-square overflow-hidden", className)}>
        <circle cx="50" cy="50" r="50" fill="currentColor" opacity="0.15" />
        <text x="50" y="50" textAnchor="middle" dominantBaseline="central" fontSize="40" fontWeight="600" fill="currentColor">
          {initials.toUpperCase()}
        </text>
      </svg>
    );
  }

  return (
    <Image
      {...rest}
      src={src}
      alt={alt}
      ref={ref}
      className={clsx("rounded-full overflow-hidden object-center object-cover aspect-square", className)}
    />
  );
});
