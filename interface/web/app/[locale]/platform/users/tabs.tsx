// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { motion } from "motion/react";
import { useLayoutEffect, useRef, useState } from "react";

import { Container, createElement, Tabs as Primitive } from "@sptlco/design";

export const Tabs = {
  ...Primitive,

  List: createElement<typeof Primitive.List>(({ value, children, ...props }: any, ref) => {
    const containerRef = useRef<HTMLDivElement | null>(null);
    const [indicator, setIndicator] = useState({
      left: 0,
      width: 0
    });

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
      <Primitive.List {...props} className={clsx("relative flex items-center sm:justify-center gap-4 mb-10 w-full", props.className)} ref={ref}>
        <Container ref={containerRef} className="relative flex items-center sm:justify-center gap-4 sm:bg-input rounded-xl w-full sm:w-auto">
          {/* Indicator sits after bg-input but before tab text in paint order */}
          <motion.span
            className="absolute bottom-0 left-0 h-full bg-button-highlight-active rounded-xl"
            animate={{
              transform: `translateX(${indicator.left}px)`,
              width: indicator.width
            }}
            transition={{
              type: "spring",
              stiffness: 500,
              damping: 40
            }}
          />
          {children}
        </Container>
      </Primitive.List>
    );
  }),

  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "relative py-2 h-10 inline-flex items-center",
        "cursor-pointer",
        "transition-all duration-500",
        "font-semibold",
        "sm:nth-2:data-[state=inactive]:pl-4",
        "sm:last:data-[state=inactive]:pr-4",
        "data-[state=inactive]:text-foreground-quaternary",
        "data-[state=active]:px-4"
      )}
    />
  )),

  Content: createElement<typeof Primitive.Content>((props, ref) => <Primitive.Content {...props} ref={ref} />)
};
