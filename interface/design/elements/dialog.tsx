// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon } from "..";
import * as Primitive from "@radix-ui/react-dialog";
import { clsx } from "clsx";

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
    <Primitive.DialogTrigger {...props} data-slot="dialog-trigger" />
  )),

  /**
   * When used, portals your overlay and content parts into the body.
   */
  Portal: createElement<typeof Primitive.Portal, Primitive.DialogPortalProps>((props, _) => (
    <Primitive.DialogPortal {...props} data-slot="dialog-portal" />
  )),

  /**
   * A layer that covers the inert portion of the view when the dialog is open.
   */
  Overlay: createElement<typeof Primitive.Overlay, Primitive.DialogOverlayProps>((props, _) => (
    <Primitive.DialogOverlay
      {...props}
      data-slot="dialog-overlay"
      className={clsx(
        "fixed inset-0 z-50 bg-black",
        "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0",
        props.className
      )}
    />
  )),

  /**
   * Contains content to be rendered in the open dialog.
   */
  Content: createElement<typeof Primitive.Content, Primitive.DialogContentProps>((props, ref) => (
    <Primitive.DialogContent
      {...props}
      ref={ref}
      data-slot="dialog-content"
      className={clsx(
        "fixed top-[50%] left-[50%] z-50 grid w-full max-w-[calc(100%-2rem)] translate-x-[-50%] translate-y-[-50%] gap-4 sm:max-w-lg",
        "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95"
      )}
    />
  )),

  /**
   * An accessible title to be announced when the dialog is opened.
   */
  Title: createElement<typeof Primitive.Title, Primitive.DialogTitleProps>((props, _) => (
    <Primitive.DialogTitle {...props} data-slot="dialog-title" />
  )),

  /**
   * An optional accessible description to be announced when the dialog is opened.
   */
  Description: createElement<typeof Primitive.Description, Primitive.DialogDescriptionProps>((props, _) => (
    <Primitive.DialogDescription {...props} data-slot="dialog-description" />
  )),

  /**
   * The button that closes the dialog.
   */
  Close: createElement<typeof Primitive.Close, Primitive.DialogCloseProps>((props, _) => (
    <Primitive.DialogClose {...props} data-slot="dialog-close" className={clsx("cursor-pointer absolute top-8 right-8", props.className)}>
      <Icon symbol="close" />
    </Primitive.DialogClose>
  ))
};
