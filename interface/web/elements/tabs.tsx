// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { motion } from "motion/react";
import { createContext, useContext, useLayoutEffect, useRef, useState } from "react";

import { Container, createElement, Tabs as Primitive, ScrollArea } from "@sptlco/design";

interface TabsContextValue {
  direction: number;
  registerTab: (value: string) => void;
}

const TabsContext = createContext<TabsContextValue>({
  direction: 0,
  registerTab: () => {}
});

export const Tabs = {
  ...Primitive,

  Root: createElement<typeof Primitive.Root>(({ onValueChange, value: controlledValue, defaultValue, children, ...props }: any, ref) => {
    const tabsRef = useRef<string[]>([]);
    const prevValueRef = useRef<string>(controlledValue ?? defaultValue ?? "");
    const [direction, setDirection] = useState(0);

    const registerTab = (tabValue: string) => {
      if (!tabsRef.current.includes(tabValue)) {
        tabsRef.current = [...tabsRef.current, tabValue];
      }
    };

    const handleValueChange = (newValue: string) => {
      const tabs = tabsRef.current;
      const prevIdx = tabs.indexOf(prevValueRef.current);
      const newIdx = tabs.indexOf(newValue);

      if (prevIdx !== -1 && newIdx !== -1) {
        setDirection(newIdx > prevIdx ? 1 : -1);
      }

      prevValueRef.current = newValue;
      onValueChange?.(newValue);
    };

    return (
      <TabsContext.Provider value={{ direction, registerTab }}>
        <Primitive.Root {...props} ref={ref} value={controlledValue} defaultValue={defaultValue} onValueChange={handleValueChange}>
          {children}
        </Primitive.Root>
      </TabsContext.Provider>
    );
  }),

  List: createElement<typeof Primitive.List>(({ value, children, ...props }: any, ref) => {
    const containerRef = useRef<HTMLDivElement | null>(null);
    const [indicator, setIndicator] = useState({ left: 0, width: 0 });

    useLayoutEffect(() => {
      const container = containerRef.current;
      if (!container) return;

      const updateIndicator = () => {
        const active = container.querySelector(`[data-state="active"]`) as HTMLElement | null;
        if (!active) return;

        const containerRect = container.getBoundingClientRect();
        const activeRect = active.getBoundingClientRect();

        setIndicator({
          left: activeRect.left - containerRect.left,
          width: activeRect.width
        });
      };

      updateIndicator();

      const mutationObserver = new MutationObserver(updateIndicator);
      mutationObserver.observe(container, {
        subtree: true,
        attributes: true,
        attributeFilter: ["data-state"]
      });

      const resizeObserver = new ResizeObserver(updateIndicator);
      container.querySelectorAll<HTMLElement>('[role="tab"]').forEach((trigger) => {
        resizeObserver.observe(trigger, { box: "border-box" });
      });

      window.addEventListener("resize", updateIndicator);

      return () => {
        mutationObserver.disconnect();
        resizeObserver.disconnect();
        window.removeEventListener("resize", updateIndicator);
      };
    }, []);

    return (
      <ScrollArea.Root className="w-full" fade fadeOrientation="horizontal">
        <ScrollArea.Viewport className="max-w-screen">
          <Primitive.List {...props} className={clsx("relative flex items-center sm:justify-center mb-10 w-full", props.className)} ref={ref}>
            <Container ref={containerRef} className="relative flex items-center sm:justify-center gap-5 sm:bg-input rounded-full w-full sm:w-auto">
              <motion.span
                className="absolute bottom-0 left-0 h-0.5 sm:h-full bg-background-inverse sm:bg-button-highlight-active sm:rounded-full"
                initial={false}
                animate={{ x: indicator.left, width: indicator.width }}
                transition={{ type: "spring", damping: 20, stiffness: 300 }}
              />
              {children}
            </Container>
          </Primitive.List>
        </ScrollArea.Viewport>
        <ScrollArea.Scrollbar className="opacity-0" orientation="horizontal" />
      </ScrollArea.Root>
    );
  }),

  Trigger: createElement<typeof Primitive.Trigger>(({ value, ...props }: any, ref) => {
    const { registerTab } = useContext(TabsContext);

    useLayoutEffect(() => {
      if (value) registerTab(value);
    }, [value]);

    return (
      <Primitive.Trigger
        {...props}
        value={value}
        ref={ref}
        className={clsx(
          "relative py-2 h-10 inline-flex items-center",
          "cursor-pointer",
          "transition-all duration-500",
          "font-semibold data-[state=active]:font-extrabold sm:data-[state=active]:font-semibold",
          "sm:nth-2:data-[state=inactive]:pl-4",
          "sm:last:data-[state=inactive]:pr-4",
          "data-[state=inactive]:text-foreground-quaternary",
          "sm:data-[state=active]:px-4"
        )}
      />
    );
  }),

  Content: createElement<typeof Primitive.Content>(({ children, ...props }: any, ref) => {
    const { direction } = useContext(TabsContext);

    return (
      <Primitive.Content {...props} ref={ref}>
        <motion.div
          initial={{ opacity: 0, x: direction * 20 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.2, ease: [0.25, 0.1, 0.25, 1] }}
        >
          {children}
        </motion.div>
      </Primitive.Content>
    );
  })
};
