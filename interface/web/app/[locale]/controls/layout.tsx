// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { Avatar, Container, Dialog, Icon, Link, Logo, Main, Sheet } from "@sptlco/design";
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
        <Container className="flex items-center gap-5">
          <Link href="/">
            <Logo mode="symbol" className="size-10 fill-foreground-primary" />
          </Link>
          <Dialog.Root>
            <Dialog.Trigger className="cursor-pointer md:hidden">
              <Icon symbol="sort" size={40} />
            </Dialog.Trigger>
            <Dialog.Content>Hello, world!</Dialog.Content>
          </Dialog.Root>
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
      <Container className={clsx("row-start-2 col-start-1", "md:col-start-2", "md:overflow-y-auto")}>
        <Container className="px-10">
          {props.children}
          <Container className="w-full h-[500vh]" />
        </Container>
        <CompactFooter className="p-10" />
      </Container>
    </Main>
  );
}
