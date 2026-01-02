// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { useUser } from "@/stores";
import { Avatar, Button, Container, Icon, Link, Logo, Main, Sheet, Span } from "@sptlco/design";
import { clsx } from "clsx";
import { useTranslations } from "next-intl";
import { ReactNode } from "react";
import { useShallow } from "zustand/shallow";

/**
 * A shared layout for administration pages.
 * @param props Configurable options for the component.
 * @returns A shared administration layout.
 */
export default function Layout(props: { children: ReactNode }) {
  const { logout, user } = useUser(useShallow((state) => ({ logout: state.logout, user: state })));
  const t = useTranslations();

  return (
    <Main className={clsx("flex flex-col w-full min-h-screen")}>
      <Container className="flex shrink-0 w-full p-10 gap-10 items-center justify-between">
        <Container className="flex items-center gap-6">
          <Link href="/" className="h-fit">
            <Logo mode="symbol" className="h-6 fill-white" />
          </Link>
          <Container className="flex items-center gap-2.5">
            <Link href="/manual" className="text-foreground-primary!">
              <Button intent="secondary" shape="pill">
                <Icon symbol="help" />
                <Span>{t("navigation.links.manual")}</Span>
              </Button>
            </Link>
            <Link href="/support" className="text-foreground-primary!">
              <Button intent="secondary" shape="pill">
                <Icon symbol="support" />
                <Span>{t("navigation.links.support")}</Span>
              </Button>
            </Link>
            <Button intent="ghost" shape="pill" className="px-2!">
              <Icon symbol="search" />
            </Button>
          </Container>
        </Container>
        <Container className="flex gap-2.5 items-center">
          <LocaleSwitcher />
          <Sheet.Root>
            <Sheet.Trigger className="cursor-pointer group">
              <Avatar
                src="https://dakarai.org/_next/image?url=%2Fdakarai.jpeg&w=3840&q=75"
                className="size-10 transition-all outline-transparent outline-0 outline-offset-2 group-data-[state=open]:outline-3 group-data-[state=open]:outline-background-highlight"
              />
            </Sheet.Trigger>
            <Sheet.Content title="Account summary" description="An overview of your account." side="right">
              {user.account?.email}
            </Sheet.Content>
          </Sheet.Root>
        </Container>
      </Container>
      <Container className="flex grow p-10">{props.children}</Container>
      <CompactFooter className="p-10" />
    </Main>
  );
}
