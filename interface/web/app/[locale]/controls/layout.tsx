// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { Avatar, Container, Dialog, Drawer, Icon, LI, Link, Logo, Main, ScrollArea, Sheet, Span, Tooltip, UL } from "@sptlco/design";
import { clsx } from "clsx";
import { useTranslations } from "next-intl";
import { ReactNode } from "react";

/**
 * A shared layout for administration pages.
 * @param props Configurable options for the component.
 * @returns A shared administration layout.
 */
export default function Layout(props: { children: ReactNode }) {
  const t = useTranslations();

  return (
    <Main className={clsx("grid w-full h-screen md:overflow-hidden", "grid-cols-1 md:grid-cols-[auto_1fr]", "grid-rows-[auto_minmax(0,1fr)]")}>
      <Container
        className={clsx(
          "p-10",
          "flex items-center md:items-start",
          "row-start-1 col-start-1",
          "md:row-span-2 md:col-start-1 md:row-start-1 md:max-w-sm",
          "md:overflow-y-auto"
        )}
      >
        <Container className="flex flex-col h-full gap-10">
          <Link href="/" className="flex items-center justify-center">
            <Logo mode="symbol" className="size-10 fill-foreground-primary" />
          </Link>
          <Drawer.Root>
            <Drawer.Trigger className="cursor-pointer md:hidden fixed rounded-full bg-translucent p-4 flex items-center justify-center z-20 bottom-10 right-10">
              <Icon symbol="apps" />
            </Drawer.Trigger>
            <Drawer.Content>Hello, world!</Drawer.Content>
          </Drawer.Root>
          <UL className="hidden md:flex grow flex-col items-center justify-center gap-6">
            <LI>
              <Tooltip.Root>
                <Tooltip.Trigger>
                  <Link
                    href="/compute"
                    className={clsx(
                      "transition-all",
                      "flex items-center justify-center size-10 scale-125",
                      "rounded-full bg-button-secondary text-foreground-primary",
                      "hover:bg-button-secondary-hover active:bg-button-secondary-active",
                      "hover:text-foreground-primary active:text-foreground-primary"
                    )}
                  >
                    <Icon symbol="bolt" fill />
                  </Link>
                </Tooltip.Trigger>
                <Tooltip.Content side="right" sideOffset={20} className="bg-translucent rounded-lg text-sm px-4 py-2">
                  Compute
                </Tooltip.Content>
              </Tooltip.Root>
            </LI>
          </UL>
        </Container>
      </Container>
      <Container className="flex p-10 gap-2.5 ml-auto items-center shrink-0 row-start-1 col-start-1 md:col-start-2">
        <LocaleSwitcher compact />
        <Sheet.Root>
          <Sheet.Trigger className="cursor-pointer group">
            <Avatar
              src="https://dakarai.org/_next/image?url=%2Fdakarai.jpeg&w=3840&q=75"
              className="size-10 transition-all outline-transparent outline-0 outline-offset-2 group-data-[state=open]:outline-3 group-data-[state=open]:outline-background-highlight"
            />
          </Sheet.Trigger>
          <Sheet.Content title={t("modals.account.title")} description={t("modals.account.description")} side="right" />
        </Sheet.Root>
      </Container>
      <ScrollArea.Root>
        <ScrollArea.Viewport className={clsx("row-start-2 col-start-1", "md:col-start-2")}>
          <Container className="px-10">
            {props.children}
            <Span className="flex w-full h-[500vh]" />
          </Container>
          <CompactFooter className="p-10" />
        </ScrollArea.Viewport>
        <ScrollArea.Scrollbar>
          <ScrollArea.Thumb />
        </ScrollArea.Scrollbar>
        <ScrollArea.Corner />
      </ScrollArea.Root>
    </Main>
  );
}
