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
  Root: createElement<
    typeof Primitive.Root,
    Primitive.ScrollAreaProps & {
      fade?: boolean;
      fadeOrientation?: "vertical" | "horizontal" | "both";
    }
  >(({ fade = false, fadeOrientation = "vertical", ...props }, ref) => {
    const [viewport, setViewport] = useState<HTMLElement | null>(null);
    const { top, bottom, left, right } = useScrollFade(viewport);

    const mask =
      fade &&
      clsx({
        "mask-t-from-(--fade-top) mask-b-from-(--fade-bottom)": fadeOrientation === "vertical" || fadeOrientation === "both",
        "mask-l-from-(--fade-left) mask-r-from-(--fade-right)": fadeOrientation === "horizontal" || fadeOrientation === "both"
      });

    return (
      <ScrollAreaViewportContext.Provider value={setViewport}>
        <Primitive.Root
          {...props}
          ref={ref}
          data-slot="scroll-area"
          style={
            {
              "--fade-top": `${top}%`,
              "--fade-bottom": `${bottom}%`,
              "--fade-left": `${left}%`,
              "--fade-right": `${right}%`
            } as React.CSSProperties
          }
          className={clsx("relative flex overflow-hidden", mask, props.className)}
        />
      </ScrollAreaViewportContext.Provider>
    );
  }),

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
    top: 100,
    bottom: 100,
    left: 100,
    right: 100
  });

  useEffect(() => {
    if (!viewport) return;

    const FADE_DISTANCE = 40;
    const FADE_POSITION = 10;

    const clamp = (v: number) => Math.max(0, Math.min(FADE_POSITION, v));

    const update = () => {
      const { scrollTop, scrollLeft, scrollHeight, scrollWidth, clientHeight, clientWidth } = viewport;

      const top = 100 - clamp((scrollTop / FADE_DISTANCE) * FADE_POSITION);

      const bottom = 100 - clamp(((scrollHeight - clientHeight - scrollTop) / FADE_DISTANCE) * FADE_POSITION);

      const left = 100 - clamp((scrollLeft / FADE_DISTANCE) * FADE_POSITION);

      const right = 100 - clamp(((scrollWidth - clientWidth - scrollLeft) / FADE_DISTANCE) * FADE_POSITION);

      setState({ top, bottom, left, right });
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
