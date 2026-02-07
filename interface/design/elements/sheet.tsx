// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Hidden, Icon, ScrollArea } from "..";
import * as Primitive from "@radix-ui/react-dialog";
import { clsx } from "clsx";
import { cva } from "cva";
import { FC, PropsWithChildren, ReactNode } from "react";

const classes = cva({
  base: [
    "fixed z-52",
    "bg-background-surface shadow-base transition ease-in-out",
    "data-[state=open]:animate-in data-[state=open]:duration-500",
    "data-[state=closed]:animate-out data-[state=closed]:duration-300"
  ],
  variants: {
    side: {
      top: ["inset-x-0 top-0 h-auto sm:rounded-b-4xl", "data-[state=open]:slide-in-from-top", "data-[state=closed]:slide-out-to-top"],
      bottom: ["inset-x-0 bottom-0 h-auto sm:rounded-t-4xl", "data-[state=open]:slide-in-from-bottom", "data-[state=closed]:slide-out-to-bottom"],
      right: [
        "inset-y-0 right-0 w-full sm:w-auto sm:max-w-full h-full sm:rounded-l-4xl",
        "data-[state=open]:slide-in-from-right",
        "data-[state=closed]:slide-out-to-right"
      ],
      left: [
        "inset-y-0 left-0 w-full sm:w-auto sm:max-w-full h-full sm:rounded-r-4xl",
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
    Primitive.DialogContentProps & { title?: ReactNode; description?: ReactNode; side?: "top" | "right" | "bottom" | "left"; closeButton?: boolean }
  >(({ side = "right", closeButton = false, ...props }, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    return (
      <Sheet.Portal>
        <Sheet.Overlay
          className={clsx(
            "fixed inset-0 z-50 size-full bg-background-base/30 backdrop-blur",
            "data-[state=open]:animate-in data-[state=open]:fade-in",
            "data-[state=closed]:animate-out data-[state=closed]:fade-out",
            "duration-500"
          )}
        />
        <Primitive.Content {...props} data-slot="sheet-content" className={clsx(classes({ side }), props.className)} ref={ref}>
          <ScrollArea.Root className="h-full rounded-[inherit]" fade>
            <ScrollArea.Viewport className="max-h-screen">
              <Container className="flex flex-col p-10 gap-10">
                <Container className="flex gap-10">
                  <Container className="flex flex-col grow">
                    <Optional value={props.title}>
                      <Primitive.Title className="font-bold text-lg">{props.title}</Primitive.Title>
                    </Optional>
                    <Optional value={props.description}>
                      <Primitive.Description className="text-foreground-secondary max-w-sm">{props.description}</Primitive.Description>
                    </Optional>
                  </Container>
                  {closeButton && (
                    <Primitive.Close className="cursor-pointer flex size-7 items-center justify-center">
                      <Icon symbol="close" />
                    </Primitive.Close>
                  )}
                </Container>
                {props.children}
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
