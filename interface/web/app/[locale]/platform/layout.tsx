// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter } from "@/elements";
import { LocaleSwitcher } from "@/locales/switch";
import { Avatar, Container, Drawer, Icon, LI, Link, Logo, Main, ScrollArea, Sheet, Span, Tooltip, UL } from "@sptlco/design";
import { clsx } from "clsx";
import { useLocale, useTranslations } from "next-intl";
import { usePathname } from "next/navigation";
import { ReactNode } from "react";

const BASE_URL = "/platform";

type Page = {
  name: string;
  path: string;
  icon: string;
};

const pages: Page[] = [
  {
    name: "navigation.platform.pages.dashboard",
    path: BASE_URL,
    icon: "view_cozy"
  },
  {
    name: "navigation.platform.pages.compute",
    path: `${BASE_URL}/compute`,
    icon: "bolt"
  },
  {
    name: "navigation.platform.pages.datastore",
    path: `${BASE_URL}/data`,
    icon: "database"
  },
  {
    name: "navigation.platform.pages.identity",
    path: `${BASE_URL}/users`,
    icon: "person"
  },
  {
    name: "navigation.platform.pages.intelligence",
    path: `${BASE_URL}/models`,
    icon: "neurology"
  },
  {
    name: "navigation.platform.pages.logistics",
    path: `${BASE_URL}/logistics`,
    icon: "package_2"
  },
  {
    name: "navigation.platform.pages.network",
    path: `${BASE_URL}/network`,
    icon: "cell_tower"
  },
  {
    name: "navigation.platform.pages.simulation",
    path: `${BASE_URL}/space`,
    icon: "simulation"
  }
];

/**
 * A shared layout for administration pages.
 * @param props Configurable options for the component.
 * @returns A shared administration layout.
 */
export default function Layout(props: { children: ReactNode }) {
  const t = useTranslations();
  const locale = useLocale();
  const path = usePathname().replace(`/${locale}`, "");

  const active = (href: string): boolean => {
    if (href === BASE_URL) {
      return path === href;
    }

    return path.startsWith(href);
  };

  return (
    <Main className={clsx("grid w-full h-screen xl:overflow-hidden", "grid-cols-1 xl:grid-cols-[auto_1fr]", "grid-rows-[auto_minmax(0,1fr)]")}>
      <Container
        className={clsx(
          "p-10",
          "flex items-center xl:items-start",
          "row-start-1 col-start-1",
          "xl:row-span-2 xl:col-start-1 xl:row-start-1 xl:max-w-sm",
          "xl:overflow-y-auto"
        )}
      >
        <Container className="flex flex-col h-full gap-10">
          <Link href="/" className="flex items-center justify-center">
            {<Logo mode="symbol" className="size-10 fill-foreground-primary" />}
          </Link>
          <Drawer.Root>
            <Drawer.Trigger className="cursor-pointer xl:hidden fixed rounded-full bg-translucent p-4 flex items-center justify-center z-20 bottom-10 right-10">
              <Icon symbol="apps" />
            </Drawer.Trigger>
            <Drawer.Content className="max-h-[80vh]">
              <ScrollArea.Root className="h-auto overflow-y-auto">
                <ScrollArea.Viewport>
                  <UL className="grid gap-4 grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
                    {pages.map((page, i) => {
                      const highlight = active(page.path);

                      return (
                        <LI key={i}>
                          <Drawer.Close asChild>
                            <Link
                              href={page.path}
                              className={clsx(
                                "transition-colors",
                                "flex flex-col gap-1 items-center justify-center w-full h-20",
                                "rounded-2xl bg-button-secondary text-foreground-primary",
                                "hover:bg-button-secondary-hover active:bg-button-secondary-active",
                                "hover:text-foreground-primary active:text-foreground-primary",
                                { "bg-blue! text-white!": highlight }
                              )}
                            >
                              <Icon symbol={page.icon} className={highlight ? "animate-fill" : "animate-outline"} />
                              <Span className="text-xs">{t(page.name)}</Span>
                            </Link>
                          </Drawer.Close>
                        </LI>
                      );
                    })}
                  </UL>
                </ScrollArea.Viewport>
                <ScrollArea.Scrollbar>
                  <ScrollArea.Thumb />
                </ScrollArea.Scrollbar>
                <ScrollArea.Corner />
              </ScrollArea.Root>
            </Drawer.Content>
          </Drawer.Root>
          <UL className="hidden xl:flex grow flex-col items-center justify-center gap-6">
            {pages.map((page, i) => {
              const highlight = active(page.path);

              return (
                <LI key={i}>
                  <Tooltip.Root>
                    <Tooltip.Trigger>
                      <Link
                        href={page.path}
                        className={clsx(
                          "transition-all",
                          "flex items-center justify-center size-10 scale-125",
                          "rounded-full bg-button-secondary text-foreground-primary",
                          "hover:bg-button-secondary-hover active:bg-button-secondary-active",
                          "hover:text-foreground-primary active:text-foreground-primary",
                          { "bg-blue! text-white! scale-140!": highlight }
                        )}
                      >
                        <Icon symbol={page.icon} className={highlight ? "animate-fill" : "animate-outline"} />
                      </Link>
                    </Tooltip.Trigger>
                    <Tooltip.Content side="right" sideOffset={20} className="bg-translucent rounded-lg text-sm px-4 py-2">
                      {t(page.name)}
                    </Tooltip.Content>
                  </Tooltip.Root>
                </LI>
              );
            })}
          </UL>
        </Container>
      </Container>
      <Container className="flex p-10 gap-2.5 sm:gap-5 ml-auto items-center shrink-0 row-start-1 col-start-1 xl:col-start-2">
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
        <ScrollArea.Viewport className={clsx("row-start-2 col-start-1", "xl:col-start-2")}>
          <Container className="px-10">{props.children}</Container>
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
