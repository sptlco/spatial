// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { motion, AnimatePresence } from "motion/react";

import { Container, createElement, ScrollArea, Span } from "@sptlco/design";

/**
 * A tree explorer element that allows the user to navigate nodes of the brain.
 */
export const Explorer = {
  /**
   * The root container. Renders panels as flex children and orchestrates
   * their enter/exit lifecycle via AnimatePresence.
   *
   * Each direct `Panel` child must carry a stable `key` so that
   * AnimatePresence can track additions and removals correctly.
   */
  Root: createElement<typeof Container>((props, ref) => {
    return (
      <Container {...props} ref={ref} className={clsx("flex h-[calc(100vh-(var(--layout-pad)*2)-80px)] flex-col", props.className)}>
        <AnimatePresence mode="sync">{props.children}</AnimatePresence>
      </Container>
    );
  }),

  /**
   * An individual panel. Animates its own layout when siblings are added or
   * removed — flex redistribution is intercepted via the `layout` prop so the
   * height change is spring-animated rather than instant.
   *
   * `overflow-hidden` ensures content is clipped cleanly while the panel
   * contracts or expands during the transition.
   */
  Panel: createElement<typeof Container, { title?: string }>(({ title, ...props }, ref) => {
    return (
      <motion.div
        ref={ref}
        layout
        style={{ transformPerspective: 900 }}
        initial={{ opacity: 0, y: 60, x: -24, scale: 0.82, rotateX: 14 }}
        animate={{ opacity: 1, y: 0, x: 0, scale: 1, rotateX: 0 }}
        exit={{ opacity: 0, y: 60, x: -24, scale: 0.82, rotateX: 14 }}
        transition={{
          layout: { type: "spring", stiffness: 800, damping: 60 },
          opacity: { duration: 0.12, ease: "easeOut" },
          default: { type: "spring", stiffness: 520, damping: 28, mass: 0.7 }
        }}
        className={clsx(
          "relative",
          "rounded-4xl bg-background-surface",
          "max-h-sm w-sm min-h-0 overflow-hidden",
          "flex flex-1 flex-col items-center",
          props.className
        )}
      >
        <motion.div layout className="shrink-0 w-full flex self-start items-center justify-center p-10 absolute inset-0">
          <Span className="flex bg-button px-4 py-2 rounded-full font-bold uppercase text-center text-xs">{title || "Panel"}</Span>
        </motion.div>
        <motion.div layout="position" className="flex flex-1 w-full flex-col min-h-0 px-1 py-10">
          <ScrollArea.Root className="size-full" fade>
            <ScrollArea.Viewport className="px-5 pt-16">{props.children}</ScrollArea.Viewport>
            <ScrollArea.Scrollbar />
          </ScrollArea.Root>
        </motion.div>
      </motion.div>
    );
  })
};
