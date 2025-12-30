// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter, Guard } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { useUser } from "@/stores";
import { Avatar, Container, Dropdown, Icon, Link, Logo, Main, Sheet, Span } from "@sptlco/design";
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
    <Main className="flex flex-col w-full min-h-screen">
      <Container className="flex shrink-0 w-full p-10 gap-10 items-center justify-between">
        <Link href="/" className="h-fit">
          <Logo mode="symbol" className="h-6 fill-white" />
        </Link>
        <Container className="flex gap-2.5 items-center">
          <LocaleSwitcher />
          <Sheet.Root>
            <Sheet.Trigger className="cursor-pointer group">
              <Avatar
                src="https://dakarai.org/_next/image?url=%2Fdakarai.jpeg&w=3840&q=75"
                className="size-10 transition-all outline-transparent outline-0 outline-offset-2 group-data-[state=open]:outline-3 group-data-[state=open]:outline-background-highlight"
              />
            </Sheet.Trigger>
            <Sheet.Content title="Your account" description="An overview of your account." side="right">
              {user.account?.email}
            </Sheet.Content>
          </Sheet.Root>
        </Container>
      </Container>
      <Container className="grow p-10"></Container>
      <CompactFooter className="p-10" />
      <Guard condition={(user) => user.authenticated} />
    </Main>
  );
}
