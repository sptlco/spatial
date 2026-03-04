// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { motion } from "motion/react";
import { useLayoutEffect, useRef, useState } from "react";

import { Container, createElement, Tabs as Primitive } from "@sptlco/design";

export const Tabs = {
  ...Primitive,

  List: createElement<typeof Primitive.List>(({ value, children, ...props }: any, ref) => {
    const listRef = useRef<HTMLDivElement | null>(null);
    const [indicator, setIndicator] = useState({
      left: 0,
      width: 0
    });

    useLayoutEffect(() => {
      const list = listRef.current;
      if (!list) return;

      const updateIndicator = () => {
        const active = list.querySelector(`[data-state="active"]`) as HTMLElement | null;

        if (!active) return;

        const listRect = list.getBoundingClientRect();
        const activeRect = active.getBoundingClientRect();

        setIndicator({
          left: activeRect.left - listRect.left,
          width: activeRect.width
        });
      };

      updateIndicator();

      // 👇 Observe attribute changes (Radix toggles data-state)
      const mutationObserver = new MutationObserver(updateIndicator);

      mutationObserver.observe(list, {
        subtree: true,
        attributes: true,
        attributeFilter: ["data-state"]
      });

      // 👇 Observe border-box so that padding transitions (px-4 on active)
      //    are tracked. The default content-box misses padding-only changes,
      //    causing the indicator to freeze at the pre-padding size on switch.
      const resizeObserver = new ResizeObserver(updateIndicator);

      list.querySelectorAll<HTMLElement>('[role="tab"]').forEach((trigger) => {
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
      <Primitive.List
        {...props}
        className={clsx("relative flex items-center sm:justify-center gap-4 mb-10 w-full", props.className)}
        ref={(node) => {
          listRef.current = node;
          if (typeof ref === "function") ref(node);
          else if (ref) (ref as any).current = node;
        }}
      >
        <Container className="flex items-center sm:justify-center gap-4 sm:bg-input rounded-xl w-full sm:w-auto">{children}</Container>
        <motion.span
          className="absolute bottom-0 left-0 h-full bg-button-highlight-active -z-10 rounded-xl"
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
      </Primitive.List>
    );
  }),

  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger
      {...props}
      ref={ref}
      className={clsx(
        "py-2 h-10 inline-flex items-center",
        "cursor-pointer",
        "transition-all duration-500",
        "font-semibold",
        "sm:first:data-[state=inactive]:pl-4",
        "sm:last:data-[state=inactive]:pr-4",
        "data-[state=inactive]:text-foreground-quaternary",
        "data-[state=active]:px-4"
      )}
    />
  )),

  Content: createElement<typeof Primitive.Content>((props, ref) => <Primitive.Content {...props} ref={ref} />)
};
