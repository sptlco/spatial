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
      const observer = new MutationObserver(updateIndicator);

      observer.observe(list, {
        subtree: true,
        attributes: true,
        attributeFilter: ["data-state"]
      });

      window.addEventListener("resize", updateIndicator);

      return () => {
        observer.disconnect();
        window.removeEventListener("resize", updateIndicator);
      };
    }, []);

    return (
      <Primitive.List
        {...props}
        className={clsx("relative flex items-center gap-2 mb-10 w-full", props.className)}
        ref={(node) => {
          listRef.current = node;
          if (typeof ref === "function") ref(node);
          else if (ref) (ref as any).current = node;
        }}
      >
        <Container className="flex items-center gap-2 bg-input rounded-xl">{children}</Container>
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
        "px-4 py-2 h-10 inline-flex items-center",
        "cursor-pointer",
        "transition-colors",
        "font-semibold",
        "data-[state=inactive]:text-foreground-quaternary"
      )}
    />
  )),

  Content: createElement<typeof Primitive.Content>((props, ref) => <Primitive.Content {...props} ref={ref} />)
};
