// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { motion, AnimatePresence } from "motion/react";

import { Accordion, Button, Container, createElement, Icon, ScrollArea, Span } from "@sptlco/design";

const TREE_INDENT = 20;

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
      <Container {...props} ref={ref} className={clsx("flex flex-col pb-10", props.className)}>
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
          "max-h-sm w-screen xl:max-w-sm min-h-0 overflow-hidden",
          "flex flex-1 flex-col items-center",
          props.className
        )}
      >
        <motion.div layout className="shrink-0 w-full flex self-start items-center justify-center p-10 absolute inset-0">
          <Span className="flex bg-button px-4 py-2 rounded-full font-bold uppercase text-center text-xs">{title || "Panel"}</Span>
        </motion.div>
        <motion.div layout="position" className="flex flex-1 w-full flex-col min-h-0 py-10">
          <ScrollArea.Root className="size-full" fade>
            <ScrollArea.Viewport className="max-h-[calc(100vh-(var(--layout-pad)*2)-160px)] pt-16">{props.children}</ScrollArea.Viewport>
            <ScrollArea.Scrollbar />
          </ScrollArea.Root>
        </motion.div>
      </motion.div>
    );
  }),

  /**
   * The accordion root for a group's neuron/synapse tree.
   * Renders as a vertical flex column; pass `Explorer.Group` elements as children.
   */
  Tree: createElement<typeof Accordion.Root, { onAdd?: () => void }>(({ onAdd, ...props }, ref) => {
    return (
      <Accordion.Root type="single" key="root" collapsible defaultValue="network" className={clsx("flex flex-col group/tree", props.className)}>
        <Accordion.Item value="network" className="flex flex-col">
          <Container className="flex items-center">
            <Explorer.Node depth={0} className="hover:bg-transparent! text-foreground-primary! uppercase text-xs font-bold">
              Hypersolver
            </Explorer.Node>
            <Container className="hidden group-hover/tree:flex gap-1.5 pr-4">
              <Button
                onClick={onAdd}
                className="size-7! p-2.5! rounded-lg text-foreground-quaternary hover:text-foreground-primary"
                size="fit"
                intent="ghost"
              >
                <Icon symbol="add" size={20} className="font-light" />
              </Button>
            </Container>
          </Container>
          <Accordion.Content>
            <Accordion.Root {...props} ref={ref}>
              {props.children}
            </Accordion.Root>
          </Accordion.Content>
        </Accordion.Item>
      </Accordion.Root>
    );
  }),

  /**
   * A collapsible accordion item representing a single neural group.
   *
   * Displays a color-coded dot strip derived from the distinct neuron types
   * present in the group, a total count badge (neurons + synapses), and a
   * chevron that rotates on open.
   *
   * @param uid The numeric group ID used as the visible label.
   * @param neurons Neurons belonging to this group; drives the type dot strip and count.
   * @param synapses Synapses belonging to this group; added to the count badge.
   */
  Group: createElement<typeof Accordion.Item, { name: string; depth?: number }>(({ name, depth = 1, children, ...props }, ref) => {
    return (
      <Accordion.Item {...props} ref={ref} className={clsx("flex flex-col", props.className)}>
        <Explorer.Node depth={depth}>{name}</Explorer.Node>
        <Accordion.Content>{children}</Accordion.Content>
      </Accordion.Item>
    );
  }),

  Node: createElement<typeof Accordion.Trigger, { depth?: number }>(({ depth = 2, children, ...props }, ref) => {
    return (
      <Accordion.Trigger
        {...props}
        ref={ref}
        style={{ paddingLeft: `${TREE_INDENT * depth + 16}px` }}
        className={clsx(
          "flex items-center group",
          "gap-1.5 pr-4 py-2 w-full",
          "text-foreground-tertiary hover:text-foreground-primary",
          "hover:bg-white/5 text-sm leading-6",
          props.className
        )}
      >
        <Icon symbol="chevron_right" size={20} className="group-data-[state=open]:rotate-90" />
        {children}
      </Accordion.Trigger>
    );
  }),

  Leaf: createElement<typeof Button, { depth?: number; onSelect?: () => void }>(({ depth = 3, onSelect, ...props }, ref) => {
    return (
      <Button
        {...props}
        ref={ref}
        intent="ghost"
        size="fill"
        shape="square"
        style={{ paddingLeft: `${TREE_INDENT * depth + 16}px` }}
        onClick={onSelect}
        className={clsx(
          "text-sm py-2! pr-4! leading-6!",
          "text-foreground-tertiary hover:text-foreground-primary",
          "hover:bg-white/5! items-start! justify-start!",
          props.className
        )}
      />
    );
  })
};
