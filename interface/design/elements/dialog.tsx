// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Hidden, Icon } from "..";
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
   * Contains content to be rendered in the open dialog.   */
  Content: createElement<typeof Primitive.Content, Primitive.DialogContentProps & { title?: ReactNode; description?: ReactNode }>((props, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    return (
      <Primitive.Portal>
        <Container className="fixed z-50 inset-0 grid grid-cols-[1fr_auto_auto] grid-rows-[auto_1fr_auto] gap-10 p-10 min-h-screen">
          <Primitive.Overlay
            data-slot="dialog-overlay"
            className={clsx(
              "fixed z-51 size-full bg-background-base/30 backdrop-blur",
              "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0"
            )}
          />
          <Container className="flex pointer-events-auto z-52 flex-col col-start-1 row-start-1 col-span-2">
            <Optional value={props.title}>
              <Primitive.Title className="font-bold text-lg">{props.title}</Primitive.Title>
            </Optional>
            <Optional value={props.description}>
              <Primitive.Description className="text-foreground-secondary">{props.description}</Primitive.Description>
            </Optional>
          </Container>
          <Primitive.Close
            data-slot="dialog-close"
            className={clsx("cursor-pointer pointer-events-auto flex z-52 col-start-3 row-start-1 h-fit justify-self-end")}
          >
            <Icon symbol="close" />
          </Primitive.Close>
          <Primitive.Content
            {...props}
            ref={ref}
            data-slot="dialog-content"
            className={clsx(
              "relative z-52 row-start-2 col-span-3 place-self-center",
              "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95",
              props.className
            )}
          >
            {props.children}
          </Primitive.Content>
        </Container>
      </Primitive.Portal>
    );
  })
};
