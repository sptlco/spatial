// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Hidden, Icon, ScrollArea } from "..";
import * as Primitive from "@radix-ui/react-dialog";
import { clsx } from "clsx";
import { FC, PropsWithChildren, ReactNode } from "react";

/**
 * A window overlaid on either the primary window or another
 * dialog window, rendering the content underneath inert.
 */
export const Dialog = {
  ...Primitive,

  /**
   * Contains all the parts of a dialog.
   */
  Root: createElement<typeof Primitive.Root, Primitive.DialogProps>((props, _) => <Primitive.Dialog {...props} data-slot="dialog" />),

  /**
   * The button that opens the dialog.
   */
  Trigger: createElement<typeof Primitive.Trigger, Primitive.DialogTriggerProps>((props, _) => (
    <Primitive.Trigger {...props} data-slot="dialog-trigger" />
  )),

  /**
   * Contains content to be rendered in the open dialog.
   */
  Content: createElement<typeof Primitive.Content, Primitive.DialogContentProps & { title?: ReactNode; description?: ReactNode }>((props, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    return (
      <Primitive.Portal>
        <Primitive.Overlay
          data-slot="dialog-overlay"
          className={clsx(
            "fixed inset-0 z-50 bg-background-base/30 backdrop-blur",
            "data-[state=open]:animate-in data-[state=open]:fade-in",
            "data-[state=closed]:animate-out data-[state=closed]:fade-out",
            "duration-500"
          )}
        />
        <Container className={clsx("fixed inset-0 z-51", "flex items-center size-full justify-center", "p-10", "pointer-events-none")}>
          <Primitive.Content
            {...props}
            ref={ref}
            data-slot="dialog-content"
            className={clsx(
              "pointer-events-auto",
              "w-full max-h-full",
              "data-[state=open]:animate-in data-[state=open]:fade-in data-[state=open]:zoom-in-95",
              "data-[state=closed]:animate-out data-[state=closed]:fade-out data-[state=closed]:zoom-out-95",
              "duration-500",
              props.className
            )}
          >
            <ScrollArea.Root className="h-full rounded-4xl items-center">
              <ScrollArea.Viewport className="max-h-[calc(100vh-80px)]">
                <Container className="flex flex-col gap-10 p-10 rounded-4xl bg-background-surface">
                  <Container className="flex items-start gap-10">
                    <Container className="flex flex-col grow">
                      <Optional value={props.title}>
                        <Primitive.Title className="font-bold text-lg">{props.title}</Primitive.Title>
                      </Optional>
                      <Optional value={props.description}>
                        <Primitive.Description className="text-foreground-secondary">{props.description}</Primitive.Description>
                      </Optional>
                    </Container>
                    <Primitive.Close data-slot="dialog-close" className="cursor-pointer flex h-fit">
                      <Icon symbol="close" />
                    </Primitive.Close>
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
        </Container>
      </Primitive.Portal>
    );
  })
};
