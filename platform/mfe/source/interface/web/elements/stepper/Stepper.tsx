// Copyright © Spatial. All rights reserved.

import {
  Div,
  Element,
  ElementProps,
  Node,
  Progress,
  Span,
} from "@spatial/elements";
import clsx from "clsx";

type StepperProps = ElementProps & {
  step?: number;
  steps: number;
};

/**
 * Create a new stepper element.
 * @param props Configurable options for the element.
 * @returns A stepper element.
 */
export const Stepper: Element<StepperProps> = ({
  step = 1,
  ...props
}: StepperProps): Node => {
  return (
    <Div
      className={clsx(
        "relative inline-flex w-full items-center",
        props.className,
      )}
    >
      <Progress
        className="!h-[1px]"
        value={Math.max(step - 1, 0)}
        max={props.steps - 1}
      />
      {Array.apply(null, Array(props.steps)).map((_o, i) => (
        <Div
          key={i}
          style={{ left: `${(i / (props.steps - 1)) * 100}%` }}
          className={clsx(
            "font-bold",
            "transition-all duration-200",
            "ease-[cubic-bezier(0.65, 0, 0.35, 1)]",
            "flex items-center justify-center",
            "bg-background-quaternary",
            "outline-background-primary outline outline-2",
            "size-1/2u absolute -translate-x-1/2 rounded-full",
            {
              "!bg-[currentColor]": step >= i + 1,
              "!size-1u !outline-none": step === i + 1,
            },
          )}
        >
          <Span
            children={i + 1}
            className={clsx("text-foreground-quaternary text-xs opacity-0", {
              "!text-base-white-9": step >= i + 1,
              "!opacity-100 duration-500": step === i + 1,
            })}
          />
        </Div>
      ))}
    </Div>
  );
};

type StepProps = ElementProps & {
  active?: boolean;
};

export const Step: Element<StepProps> = (props: StepProps): Node => {
  return null;
};
