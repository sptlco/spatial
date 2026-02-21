// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Footer, LocaleSwitcher } from "@/elements";
import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
import { clsx } from "clsx";
import { useLocale, useTranslations } from "next-intl";
import { usePathname } from "next/navigation";
import { FormEvent, ReactNode, useEffect, useRef, useState } from "react";
import { SWRConfig } from "swr";
import { useShallow } from "zustand/shallow";

import {
  Avatar,
  Button,
  Container,
  Dialog,
  Drawer,
  Field,
  Form,
  Icon,
  LI,
  Link,
  Logo,
  Main,
  ScrollArea,
  Separator,
  Sheet,
  Span,
  Spinner,
  toast,
  Tooltip,
  UL
} from "@sptlco/design";

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
    name: "navigation.platform.pages.environment",
    path: `${BASE_URL}/space`,
    icon: "precision_manufacturing"
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
    icon: "compare_arrows"
  }
];

type Requirements = {
  name: boolean;
};

/**
 * A shared layout for administration pages.
 * @param props Configurable options for the component.
 * @returns A shared administration layout.
 */
export default function Layout(props: { children: ReactNode }) {
  const t = useTranslations();
  const locale = useLocale();
  const path = usePathname().replace(`/${locale}`, "");

  const user = {
    ...useUser(
      useShallow((state) => ({
        loading: state.loading,
        account: state.account
      }))
    ),
    authenticated: useUser((state) => state.authenticated),
    update: useUser((state) => state.update),
    logout: useUser((state) => state.logout)
  };

  const active = (href: string): boolean => {
    if (href === BASE_URL) {
      return path === href;
    }

    return path.startsWith(href);
  };

  const scroller = useRef<HTMLDivElement>(null);
  const frame = useRef<number>(null);

  useEffect(() => {
    const el = scroller.current;

    if (!el) {
      return;
    }

    const onScroll = () => {
      if (frame.current) {
        return;
      }

      frame.current = requestAnimationFrame(() => {
        const { scrollTop } = el;

        document.documentElement.style.setProperty("--layout-pad", `${Math.max(16, 40 - (scrollTop * 3) / 40)}px`);

        frame.current = null;
      });
    };

    onScroll();

    el.addEventListener("scroll", onScroll, { passive: true });

    return () => {
      el.removeEventListener("scroll", onScroll);

      if (frame.current) {
        cancelAnimationFrame(frame.current);
      }
    };
  }, []);

  const [requirements, setRequirements] = useState<Requirements>({ name: false });
  const [update, setUpdate] = useState<Account>();

  useEffect(() => {
    if (!user.loading) {
      if (user.authenticated()) {
        setTimeout(() => {
          setRequirements((value) => ({ ...value, name: !user.account.name }));
        }, 1000);
      }
    }
  }, [user.loading, user.account, user.authenticated]);

  const submit = async (e: FormEvent) => {
    e.preventDefault();

    if (!update) {
      return;
    }

    toast.promise(Spatial.accounts.update(update), {
      loading: "Updating account",
      description: "We are updating your account with the information you provided.",
      success: (response) => {
        if (!response.error) {
          user.update({
            account: {
              ...user.account,
              name: update.name
            }
          });

          return {
            message: "Account updated",
            description: "Your account has been updated."
          };
        }

        return {
          message: "Something went wrong",
          description: "An error occurred while updating your account."
        };
      }
    });
  };

  const logout = async () => {
    await user.logout();
    window.location.reload();
  };

  const loading = user.loading || !user.authenticated();

  return (
    <SWRConfig
      value={{
        refreshInterval: 0, // disable polling unless needed
        revalidateOnFocus: false,
        revalidateOnReconnect: false,
        dedupingInterval: 10000,
        shouldRetryOnError: false
      }}
    >
      <Main className={clsx("grid w-full h-screen", "grid-cols-1 xl:grid-cols-[auto_1fr]", "grid-rows-[auto_minmax(0,1fr)]")}>
        <Dialog.Root open={requirements.name}>
          <Dialog.Content title="Information required" description="We need to know the following." closeButton={false}>
            <Form className="flex flex-col items-center gap-10" onSubmit={submit}>
              <Field
                type="text"
                id="name"
                name="name"
                label="Name"
                placeholder="What is your name?"
                value={update?.name || ""}
                onChange={(e) => setUpdate({ ...user.account, name: e.target.value })}
              />
              <Button type="submit" disabled={!update?.name} className="w-full">
                Submit
              </Button>
            </Form>
          </Dialog.Content>
        </Dialog.Root>
        <Container
          style={{
            paddingTop: "var(--layout-pad)",
            paddingBottom: "var(--layout-pad)"
          }}
          className={clsx(
            "px-10",
            "flex items-center xl:items-start",
            "row-start-1 col-start-1",
            "xl:row-span-2 xl:col-start-1 xl:row-start-1 xl:max-w-sm",
            "xl:overflow-y-auto"
          )}
        >
          <Container className="flex h-full gap-0 xl:gap-10">
            <Container className="flex flex-col h-full gap-10">
              <Link href={BASE_URL} className="relative flex items-center justify-center size-10 xl:w-16!">
                <Logo mode="symbol" className="w-10 fill-foreground-primary" />
              </Link>
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
                              "transition-all relative",
                              "flex items-center justify-center size-16",
                              "rounded-2xl bg-button-ghost text-foreground-primary",
                              "hover:bg-button-ghost-hover active:bg-button-ghost-active",
                              "hover:text-foreground-primary active:text-foreground-primary",
                              { "bg-button-ghost-hover! text-white!": highlight }
                            )}
                          >
                            <Icon symbol={page.icon} className={highlight ? "animate-fill" : "animate-outline"} />
                            {highlight && <Span className="absolute bottom-2.5 size-1 translate-y-1/4 rounded-full bg-blue" />}
                          </Link>
                        </Tooltip.Trigger>
                        <Tooltip.Content side="right" sideOffset={20}>
                          {t(page.name)}
                        </Tooltip.Content>
                      </Tooltip.Root>
                    </LI>
                  );
                })}
              </UL>
            </Container>
            <Separator orientation="vertical" className="hidden xl:flex bg-linear-to-b from-transparent to-transparent via-line-subtle h-full w-px" />
          </Container>
        </Container>
        <Container
          className={clsx("flex px-10 gap-2.5 sm:gap-5 ml-auto items-center shrink-0", "row-start-1 col-start-1 xl:col-start-2")}
          style={{
            paddingTop: "var(--layout-pad)",
            paddingBottom: "var(--layout-pad)"
          }}
        >
          <Container className="flex items-center gap-2.5">
            <Button intent="ghost" className="p-0! size-10!">
              <Icon symbol="search" />
            </Button>
            <LocaleSwitcher compact />
          </Container>
          <Sheet.Root>
            <Sheet.Trigger className="cursor-pointer group">
              {loading ? (
                <Span className="flex bg-background-surface size-10 rounded-full animate-pulse" />
              ) : (
                <Avatar
                  src={user.account.avatar}
                  alt={user.account.name}
                  className="size-10 transition-all outline-transparent outline-0 outline-offset-2 group-data-[state=open]:outline-3 group-data-[state=open]:outline-background-highlight"
                />
              )}
            </Sheet.Trigger>
            <Sheet.Content title={t("modals.account.title")} description={t("modals.account.description")} closeButton side="right">
              <Button size="fill" disabled={loading} onClick={logout}>
                <Span>Logout</Span>
                <Span className="flex size-5 items-center justify-center">
                  {loading ? <Spinner className="size-3.5 text-hint" /> : <Icon symbol="arrow_right_alt" size={20} />}
                </Span>
              </Button>
            </Sheet.Content>
          </Sheet.Root>
        </Container>
        <ScrollArea.Root fade>
          <ScrollArea.Viewport ref={scroller} className={clsx("xl:pr-10", "row-start-2 col-start-1", "xl:col-start-2")}>
            <Container className="flex flex-col relative xl:min-h-[calc(100vh-(var(--layout-pad)*2)-140px)]">{props.children}</Container>
            <Footer className="p-10" />
          </ScrollArea.Viewport>
          <ScrollArea.Scrollbar />
          <ScrollArea.Corner />
        </ScrollArea.Root>
        <Container id="actions" className="fixed pointer-events-none bottom-0 left-0 w-full flex gap-5 p-10 z-20 justify-start">
          <Drawer.Root>
            <Drawer.Trigger className="xl:hidden z-30 pointer-events-auto cursor-pointer rounded-full bg-background-surface shadow-base p-4 flex shrink-0 items-center justify-center">
              <Icon symbol="apps" />
            </Drawer.Trigger>
            <Drawer.Content className="max-h-[80vh]">
              <ScrollArea.Root className="h-auto overflow-y-auto" fade>
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
                                "rounded-2xl bg-button text-foreground-primary",
                                "hover:bg-button-hover active:bg-button-active",
                                "hover:text-foreground-primary active:text-foreground-primary",
                                { "bg-button-highlight-active! text-white!": highlight }
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
        </Container>
      </Main>
    </SWRConfig>
  );
}
