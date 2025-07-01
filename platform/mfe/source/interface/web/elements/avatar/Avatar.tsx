// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { cva } from "class-variance-authority";
import { clsx } from "clsx";
import { useEffect, useState } from "react";

import { AvatarProps, Element, Node, Skeleton, Span, Spinner } from "..";

const classes = cva(["rounded-full flex items-center justify-center"], {
  variants: {
    mode: {
      text: [],
      picture: ["object-cover"],
    },
  },
});

/**
 * Create a new avatar element.
 * @param props Configurable options for the element.
 * @returns An avatar element.
 */
export const Avatar: Element<AvatarProps> = (props: AvatarProps): Node => {
  if (props.mode == "text") {
    if (!props.text) {
      throw "Text mode requires the text property to be specified.";
    }

    const abbreviate = (text: string): string => {
      return text.charAt(0).toUpperCase();
    };

    const bg = (text: string, saturation: number, luminosity: number) => {
      let hash = 0;

      for (let i = 0; i < text.length; i++) {
        hash = text.charCodeAt(i) + ((hash << 5) - hash);
      }

      return {
        h: ((((hash % 360) + 360) % 360) + 308) % 360,
        s: saturation,
        l: luminosity,
      };
    };

    const background = bg(props.text, 91, 53);

    return (
      <svg
        ref={props.ref}
        xmlns="http://www.w3.org/2000/svg"
        style={props.style}
        className={clsx(classes({ mode: props.mode }), props.className)}
        viewBox="0 0 64 64"
        version="1.1"
      >
        <circle
          fill={`hsl(${background.h}, ${background.s}%, ${background.l}%)`}
          width="64"
          height="64"
          cx="32"
          cy="32"
          r="32"
        />
        <text
          x="50%"
          y="50%"
          alignmentBaseline="middle"
          textAnchor="middle"
          fontWeight="400"
          fontSize="24"
          dy=".1em"
          dominantBaseline="middle"
          fill="#FFFFFF"
        >
          {abbreviate(props.text)}
        </text>
      </svg>
    );
  }

  return (
    <Span
      className={clsx(
        classes({ mode: props.mode }),
        "relative",
        props.className,
      )}
    >
      <img
        ref={props.ref}
        src={props.url}
        className={clsx(classes({ mode: props.mode }), props.className)}
      />
    </Span>
  );
};
