// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Container, createElement, Hidden, Icon, Span } from "..";
import { clsx } from "clsx";
import { ComponentProps, FC, PropsWithChildren, ReactNode, useEffect, useState } from "react";
import { createPortal } from "react-dom";
import { Drawer as Primitive } from "vaul";

export const Drawer = {
  ...Primitive,

  Root: createElement<typeof Primitive.Root, ComponentProps<typeof Primitive.Root>>((props, _) => <Primitive.Root {...props} />),

  Trigger: createElement<typeof Primitive.Trigger, ComponentProps<typeof Primitive.Trigger>>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} />
  )),

  Header: createElement<typeof Container>((props, ref) => <Container {...props} ref={ref} className={clsx("flex flex-col", props.className)} />),

  Content: createElement<
    typeof Primitive.Content,
    ComponentProps<typeof Primitive.Content> & {
      title?: ReactNode;
      description?: ReactNode;
      overlay?: boolean;
      closeButton?: boolean;
      nested?: boolean;
    }
  >(({ nested, overlay = true, ...props }, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    return (
      <Primitive.Portal>
        <Primitive.Overlay
          data-slot="drawer-overlay"
          className={clsx("fixed inset-0 size-full", nested ? "z-(--z-nested-overlay)" : "z-(--z-overlay)", {
            "bg-black/30 backdrop-blur": overlay,
            "data-[state=open]:animate-in data-[state=open]:fade-in": overlay,
            "data-[state=closed]:animate-out data-[state=closed]:fade-out": overlay,
            "duration-500": overlay
          })}
        />
        <Primitive.Content
          {...props}
          ref={ref}
          data-slot="drawer-content"
          className={clsx(
            "group/drawer-content bg-background-surface border-line-faint fixed p-10 gap-10 flex h-auto flex-col",
            "data-[vaul-drawer-direction=top]:border-b",
            "data-[vaul-drawer-direction=bottom]:border-t",
            "data-[vaul-drawer-direction=left]:border-r",
            "data-[vaul-drawer-direction=right]:border-l",
            nested ? "z-(--z-nested)" : "z-(--z-sheet)",
            "data-[vaul-drawer-direction=top]:inset-x-0 data-[vaul-drawer-direction=top]:top-0 data-[vaul-drawer-direction=top]:mb-24 data-[vaul-drawer-direction=top]:max-h-[80vh] data-[vaul-drawer-direction=top]:rounded-b-4xl",
            "data-[vaul-drawer-direction=bottom]:inset-x-0 data-[vaul-drawer-direction=bottom]:bottom-0 data-[vaul-drawer-direction=bottom]:mt-24 data-[vaul-drawer-direction=bottom]:max-h-[80vh] data-[vaul-drawer-direction=bottom]:rounded-t-4xl",
            "data-[vaul-drawer-direction=right]:inset-y-0 data-[vaul-drawer-direction=right]:right-0 data-[vaul-drawer-direction=right]:w-3/4 data-[vaul-drawer-direction=right]:rounded-l-4xl data-[vaul-drawer-direction=right]:sm:max-w-sm",
            "data-[vaul-drawer-direction=left]:inset-y-0 data-[vaul-drawer-direction=left]:left-0 data-[vaul-drawer-direction=left]:w-3/4 data-[vaul-drawer-direction=right]:rounded-r-4xl data-[vaul-drawer-direction=left]:sm:max-w-sm"
          )}
        >
          <Container className="grid grid-cols-3">
            <Container className="flex grow flex-col col-start-1">
              <Optional value={props.title}>
                <Primitive.Title className="font-bold text-lg">{props.title}</Primitive.Title>
              </Optional>
              <Optional value={props.description}>
                <Primitive.Description className="text-foreground-secondary">{props.description}</Primitive.Description>
              </Optional>
            </Container>
            <Span className="col-start-2 place-self-center bg-translucent hidden h-1.5 w-[100px] shrink-0 rounded-full group-data-[vaul-drawer-direction=bottom]/drawer-content:block" />
            {props.closeButton && (
              <Primitive.Close data-slot="drawer-close" className={clsx("col-start-3 cursor-pointer inline-flex h-fit justify-self-end")}>
                <Icon symbol="close" />
              </Primitive.Close>
            )}
          </Container>
          {props.children}
        </Primitive.Content>
      </Primitive.Portal>
    );
  })
};

/**
 * Consume a drawer.
 * @param drawer The drawer to consume.
 * @returns Controls for the drawer.
 */
export const useDrawer = (drawer: ReactNode) => {
  const [open, setOpen] = useState(false);

  useEffect(() => {
    createPortal(
      <Drawer.Root open={open} onOpenChange={setOpen}>
        {drawer}
      </Drawer.Root>,
      document.body
    );
  }, []);

  return {
    open: () => setOpen(true),
    close: () => setOpen(false)
  };
};
