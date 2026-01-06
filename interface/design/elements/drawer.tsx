// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Hidden, Icon, Span } from "..";
import { clsx } from "clsx";
import { ComponentProps, FC, PropsWithChildren, ReactNode } from "react";
import { Drawer as Primitive } from "vaul";

export const Drawer = {
  ...Primitive,

  Root: createElement<typeof Primitive.Root, ComponentProps<typeof Primitive.Root>>((props, _) => <Primitive.Root {...props} />),

  Trigger: createElement<typeof Primitive.Trigger, ComponentProps<typeof Primitive.Trigger>>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} />
  )),

  Content: createElement<
    typeof Primitive.Content,
    ComponentProps<typeof Primitive.Content> & { title?: ReactNode; description?: ReactNode; closeButton?: boolean }
  >((props, ref) => {
    const Optional: FC<PropsWithChildren<{ value?: ReactNode }>> = ({ value, ...props }) => {
      return !value ? <Hidden {...props} /> : props.children;
    };

    return (
      <Primitive.Portal>
        <Primitive.Overlay
          data-slot="drawer-overlay"
          className={clsx(
            "fixed inset-0 z-50 size-full bg-background-base/30 backdrop-blur",
            "data-[state=open]:animate-in data-[state=open]:fade-in",
            "data-[state=closed]:animate-out data-[state=closed]:fade-out",
            "duration-500"
          )}
        />
        <Primitive.Content
          {...props}
          ref={ref}
          data-slot="drawer-content"
          className={clsx(
            "group/drawer-content bg-background-surface fixed z-50 p-10 gap-10 flex h-auto flex-col",
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
