// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Container, createElement, Hidden, Icon, Paragraph, ScrollArea } from "..";
import * as Primitive from "@radix-ui/react-dialog";
import { clsx } from "clsx";
import { cva } from "cva";
import { FC, PropsWithChildren, ReactNode } from "react";

const classes = cva({
  base: [
    "fixed z-(--z-sheet)",
    "bg-background-surface shadow-base transition ease-in-out",
    "data-[state=open]:animate-in data-[state=open]:duration-500",
    "data-[state=closed]:animate-out data-[state=closed]:duration-300"
  ],
  variants: {
    side: {
      top: ["inset-x-0 top-0 h-auto sm:rounded-b-4xl", "data-[state=open]:slide-in-from-top", "data-[state=closed]:slide-out-to-top"],
      bottom: ["inset-x-0 bottom-0 h-auto sm:rounded-t-4xl", "data-[state=open]:slide-in-from-bottom", "data-[state=closed]:slide-out-to-bottom"],
      right: [
        "inset-y-0 right-0 w-full sm:w-fit max-w-full h-full sm:rounded-l-4xl",
        "data-[state=open]:slide-in-from-right",
        "data-[state=closed]:slide-out-to-right"
      ],
      left: [
        "inset-y-0 left-0 w-full sm:w-fit max-w-full h-full sm:rounded-r-4xl",
        "data-[state=open]:slide-in-from-left",
        "data-[state=closed]:slide-out-to-left"
      ]
    }
  }
});

/**
 * Extends the dialog element to display content that complements
 * the main content of the screen.
 */
export const Sheet = {
  Root: createElement<typeof Primitive.Root, Primitive.DialogProps>((props, _) => <Primitive.Root {...props} />),

  Trigger: createElement<typeof Primitive.Trigger, Primitive.DialogTriggerProps>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} data-slot="sheet-trigger" />
  )),

  Close: createElement<typeof Primitive.Close, Primitive.DialogCloseProps>((props, ref) => <Primitive.Close {...props} ref={ref} />),

  Portal: createElement<typeof Primitive.Portal, Primitive.DialogPortalProps>((props, _) => <Primitive.Portal {...props} />),

  Overlay: createElement<typeof Primitive.Overlay, Primitive.DialogOverlayProps>((props, ref) => <Primitive.Overlay {...props} ref={ref} />),

  Content: createElement<
    typeof Primitive.Content,
    Primitive.DialogContentProps & {
      overlay?: boolean;
      title?: ReactNode;
      description?: ReactNode;
      side?: "top" | "right" | "bottom" | "left";
      closeButton?: boolean;
    }
  >(({ side = "right", overlay = true, closeButton = false, title, description, ...props }, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    const horizontal = side === "left" || side === "right";

    return (
      <Sheet.Portal>
        <Sheet.Overlay
          className={clsx("fixed inset-0 z-(--z-overlay) size-full", {
            "bg-black/30 backdrop-blur": overlay,
            "data-[state=open]:animate-in data-[state=open]:fade-in": overlay,
            "data-[state=closed]:animate-out data-[state=closed]:fade-out": overlay,
            "duration-500": overlay
          })}
        />
        <Primitive.Content
          {...props}
          data-slot="sheet-content"
          className={clsx(classes({ side }), "will-change-transform", props.className)}
          ref={ref}
        >
          <ScrollArea.Root className="h-full rounded-[inherit]" fade>
            <ScrollArea.Viewport className="max-h-screen">
              <Container className={clsx("w-full flex flex-col p-10 gap-10", horizontal && "min-h-screen")}>
                <Optional value={title || description || closeButton}>
                  <Container className="w-full flex flex-col gap-2.5">
                    <Container className="flex items-center">
                      <Optional value={title}>
                        <Primitive.Title className="font-bold text-lg flex-1 min-w-0 truncate">{title}</Primitive.Title>
                      </Optional>
                      <Optional value={closeButton}>
                        <Primitive.Close className="cursor-pointer size-7 items-center justify-center">
                          <Icon symbol="close" />
                        </Primitive.Close>
                      </Optional>
                    </Container>
                    <Optional value={description}>
                      <Primitive.Description className="text-foreground-secondary w-full sm:max-w-sm" asChild>
                        <Paragraph>{description}</Paragraph>
                      </Primitive.Description>
                    </Optional>
                  </Container>
                </Optional>
                <Container className={clsx(horizontal && "flex-1 flex flex-col gap-10")}>{props.children}</Container>
              </Container>
            </ScrollArea.Viewport>
            <ScrollArea.Scrollbar>
              <ScrollArea.Thumb />
            </ScrollArea.Scrollbar>
            <ScrollArea.Corner />
          </ScrollArea.Root>
        </Primitive.Content>
      </Sheet.Portal>
    );
  })
};
