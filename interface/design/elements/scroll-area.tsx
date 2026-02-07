// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-scroll-area";
import { clsx } from "clsx";
import { cva } from "cva";
import { createContext, useContext, useEffect, useState } from "react";

/**
 * Augments native scroll functionality for custom, cross-browser styling.
 */
export const ScrollArea = {
  /**
   * Contains all the parts of a scroll area.
   */
  Root: createElement<typeof Primitive.Root, Primitive.ScrollAreaProps & { fade?: boolean; fadeClassName?: string }>(
    ({ fade = false, ...props }, ref) => {
      const [viewport, setViewport] = useState<HTMLElement | null>(null);
      const { up, down } = useScrollFade(viewport);

      return (
        <ScrollAreaViewportContext.Provider value={setViewport}>
          <Primitive.Root
            {...props}
            ref={ref}
            data-slot="scroll-area"
            style={
              {
                "--fade-top": `${up}%`,
                "--fade-bottom": `${down}%`
              } as React.CSSProperties
            }
            className={clsx("relative flex overflow-hidden", { "mask-t-from-(--fade-top) mask-b-from-(--fade-bottom)": fade }, props.className)}
          />
        </ScrollAreaViewportContext.Provider>
      );
    }
  ),

  /**
   * The viewport area of the scroll area.
   */
  Viewport: createElement<typeof Primitive.Viewport, Primitive.ScrollAreaViewportProps>((props, ref) => {
    const register = useContext(ScrollAreaViewportContext);

    return (
      <Primitive.Viewport
        {...props}
        data-slot="scroll-area-viewport"
        className={clsx("size-full", props.className)}
        ref={(node) => {
          if (typeof ref === "function") {
            ref(node);
          } else if (ref) {
            ref.current = node;
          }

          register?.(node);
        }}
      />
    );
  }),

  /**
   * The vertical scrollbar.
   */
  Scrollbar: createElement<typeof Primitive.Scrollbar, Primitive.ScrollAreaScrollbarProps>(({ orientation = "vertical", ...props }, ref) => {
    const classes = cva({
      base: ["group", "flex touch-none select-none", "transition-colors", "p-px"],
      variants: {
        orientation: {
          vertical: "w-1.5",
          horizontal: "h-1.5 flex-col"
        }
      }
    });

    return (
      <Primitive.Scrollbar
        {...props}
        ref={ref}
        orientation={orientation}
        data-slot="scroll-area-scrollbar"
        className={clsx(classes({ orientation }), props.className)}
      >
        <ScrollArea.Thumb />
      </Primitive.Scrollbar>
    );
  }),

  /**
   * The thumb to be used in a scrollbar.
   */
  Thumb: createElement<typeof Primitive.Thumb, Primitive.ScrollAreaThumbProps>((props, ref) => (
    <Primitive.Thumb
      {...props}
      ref={ref}
      data-slot="scroll-area-thumb"
      className={clsx("transition-colors", "rounded-full relative flex-1", "bg-button group-hover:bg-button-hover", props.className)}
    />
  )),

  /**
   * The corner where both vertical and horizontal scrollbars meet.
   */
  Corner: createElement<typeof Primitive.Corner, Primitive.ScrollAreaCornerProps>((props, ref) => (
    <Primitive.Corner {...props} ref={ref} data-slot="scroll-area-corner" />
  ))
};

const ScrollAreaViewportContext = createContext<((el: HTMLElement | null) => void) | null>(null);

function useScrollFade(viewport?: HTMLElement | null) {
  const [state, setState] = useState({
    up: 0,
    down: 0
  });

  useEffect(() => {
    if (!viewport) {
      return;
    }

    const FADE_DISTANCE = 40;
    const FADE_POSITION = 10;

    const clamp = (v: number) => Math.max(0, Math.min(FADE_POSITION, v));

    const update = () => {
      const { scrollTop, scrollHeight, clientHeight } = viewport;

      const top = 100 - clamp((scrollTop / FADE_DISTANCE) * FADE_POSITION);
      const bottom = 100 - clamp(((scrollHeight - clientHeight - scrollTop) / FADE_DISTANCE) * FADE_POSITION);

      setState({ up: top, down: bottom });
    };

    update();

    viewport.addEventListener("scroll", update, { passive: true });
    window.addEventListener("resize", update);

    return () => {
      viewport.removeEventListener("scroll", update);
      window.removeEventListener("resize", update);
    };
  }, [viewport]);

  return state;
}
