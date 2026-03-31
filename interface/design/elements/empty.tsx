// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { cva } from "cva";

import { Container, createElement } from "..";

/**
 * An element that displays an empty state.
 */
export const Empty = {
  Root: createElement<typeof Container>((props, ref) => (
    <Container
      {...props}
      ref={ref}
      className={clsx(
        "flex w-full min-w-0 flex-1 flex-col items-center justify-center gap-4",
        "rounded-xl border-dashed p-10 text-center text-balance",
        props.className
      )}
    />
  )),

  Header: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} className={clsx("flex max-w-sm flex-col items-center gap-2", props.className)} />
  )),

  Media: createElement<typeof Container, { variant?: "default" | "icon" }>(({ variant = "default", ...props }, ref) => {
    const variants = cva({
      base: "mb-2 flex shrink-0 items-center justify-center [&_svg]:pointer-events-none [&_svg]:shrink-0",
      variants: {
        variant: {
          default: "bg-transparent",
          icon: "flex size-10 shrink-0 items-center justify-center rounded-lg bg-input text-foreground-primary [&_svg:not([class*='size-'])]:size-4"
        }
      },
      defaultVariants: {
        variant: "default"
      }
    });

    return <Container {...props} ref={ref} className={clsx(variants({ variant }), props.className)} />;
  }),

  Title: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} className={clsx("text-sm font-medium tracking-tight", props.className)} />
  )),

  Description: createElement<typeof Container>((props, ref) => (
    <Container
      {...props}
      ref={ref}
      className={clsx(
        "text-sm/relaxed text-foreground-quaternary [&>a]:underline [&>a]:underline-offset-4 [&>a:hover]:text-foreground-primary",
        props.className
      )}
    />
  )),

  Content: createElement<typeof Container>((props, ref) => (
    <Container
      {...props}
      ref={ref}
      className={clsx("flex w-full max-w-sm min-w-0 flex-col items-center gap-2.5 text-sm text-balance", props.className)}
    />
  ))
};
