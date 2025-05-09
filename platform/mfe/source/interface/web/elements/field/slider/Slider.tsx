// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-slider";
import { Element, Field, Node, SliderProps } from "../..";
import clsx from "clsx";

/**
 * Create a new slider element.
 * @param props Configurable options for the element.
 * @returns A slider element.
 */
export const Slider: Element<SliderProps> = ({
  orientation = "horizontal",
  ...props
}: SliderProps): Node => {
  return (
    <Field
      ref={props.ref}
      style={props.style}
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={clsx(
        { "w-fit items-center": orientation == "vertical" },
        props.className,
      )}
    >
      <Primitive.Root
        value={props.value}
        onValueChange={props.onChange}
        onValueCommit={props.onCommit}
        name={props.name}
        min={props.min}
        max={props.max}
        step={props.step}
        orientation={orientation}
        disabled={props.disabled}
        className={clsx(
          "grow",
          "relative flex touch-none select-none items-center",
          "data-[orientation=horizontal]:h-1u",
          "data-[orientation=vertical]:w-1u data-[orientation=vertical]:flex-col",
        )}
      >
        <Primitive.Track
          className={clsx(
            "grow",
            "relative",
            "bg-background-quaternary rounded-full",
            "data-[orientation=horizontal]:h-1/4u",
            "data-[orientation=vertical]:w-1/4u",
          )}
        >
          <Primitive.Range
            className={clsx(
              "absolute",
              "rounded-full",
              "bg-background-interactive-accent-default",
              "data-[orientation=horizontal]:h-full",
              "data-[orientation=vertical]:w-full",
            )}
          />
        </Primitive.Track>
        <Primitive.Thumb
          className={clsx(
            "size-1u flex",
            "transition-all duration-100",
            "bg-background-primary rounded-full",
            "outline-background-interactive-accent-default outline outline-1 focus:outline-2",
          )}
        />
      </Primitive.Root>
    </Field>
  );
};
