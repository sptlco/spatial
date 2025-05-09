// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-progress";
import { Element, Node, ProgressProps } from "..";
import clsx from "clsx";

/**
 * Create a new progress element.
 * @param props Configurable options for the element.
 * @returns A progress element.
 */
export const Progress: Element<ProgressProps> = ({
  value = 0,
  max = 100,
  ...props
}: ProgressProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      value={value}
      max={max}
      style={{ transform: "translateZ(0)", ...props.style }}
      className={clsx(
        "relative",
        "h-1/4u flex w-full items-center overflow-hidden",
        "bg-background-quaternary rounded-full",
        props.className,
      )}
    >
      <Primitive.Indicator
        style={{ transform: `translateX(-${((max - value) / max) * 100}%)` }}
        className={clsx(
          "absolute size-full",
          "bg-[currentColor]",
          "ease-[cubic-bezier(0.65, 0, 0.35, 1)] transition-transform duration-500",
        )}
      />
      {props.children}
    </Primitive.Root>
  );
};
