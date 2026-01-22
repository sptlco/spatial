// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { CompactFooter, LocaleSwitcher } from "@/elements";
import { useUser } from "@/stores";
import { Spatial } from "@sptlco/client";
import { Account } from "@sptlco/data";
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
  Sheet,
  Span,
  toast,
  Tooltip,
  UL
} from "@sptlco/design";
import { clsx } from "clsx";
import { useLocale, useTranslations } from "next-intl";
import { usePathname } from "next/navigation";
import { FormEvent, ReactNode, useEffect, useRef, useState } from "react";
import { useShallow } from "zustand/shallow";

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
  const user = useUser(
    useShallow((state) => ({
      loading: state.loading,
      authenticated: state.authenticated,
      account: state.account,
      update: state.update
    }))
  );

  const active = (href: string): boolean => {
    if (href === BASE_URL) {
      return path === href;
    }

    return path.startsWith(href);
  };

  const scroller = useRef<HTMLDivElement>(null);
  const raf = useRef<number>(null);

  useEffect(() => {
    const el = scroller.current;

    if (!el) {
      return;
    }

    const onScroll = () => {
      if (raf.current) {
        return;
      }

      raf.current = requestAnimationFrame(() => {
        const scrollTop = el.scrollTop;
        const padding = Math.max(16, 40 - (scrollTop * 3) / 40);

        document.documentElement.style.setProperty("--layout-pad", `${padding}px`);

        raf.current = null;
      });
    };

    el.addEventListener("scroll", onScroll, { passive: true });

    return () => {
      el.removeEventListener("scroll", onScroll);

      if (raf.current) {
        cancelAnimationFrame(raf.current);
      }
    };
  }, []);

  const [requirements, setRequirements] = useState<Requirements>({ name: false });
  const [update, setUpdate] = useState<Account>();

  useEffect(() => {
    if (!user.loading && user.authenticated()) {
      setTimeout(() => {
        setRequirements((value) => ({ ...value, name: !user.account.name }));
      }, 1000);
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

  return (
    <Main className={clsx("grid w-full h-screen xl:overflow-hidden", "grid-cols-1 xl:grid-cols-[auto_1fr]", "grid-rows-[auto_minmax(0,1fr)]")}>
      <Drawer.Root>
        <Drawer.Trigger className="cursor-pointer xl:hidden fixed rounded-full bg-background-surface shadow-lg p-4 flex items-center justify-center z-20 bottom-10 right-10">
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
      <Dialog.Root open={requirements.name}>
        <Dialog.Content className="sm:max-w-md" title="Information required" description="We need to know the following." closeButton={false}>
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
        <Container className="flex flex-col h-full gap-10">
          <Link href="/" className="flex items-center justify-center">
            {<Logo mode="symbol" className="size-10 fill-foreground-primary" />}
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
                    <Tooltip.Content side="right" sideOffset={20}>
                      {t(page.name)}
                    </Tooltip.Content>
                  </Tooltip.Root>
                </LI>
              );
            })}
          </UL>
        </Container>
      </Container>
      <Container
        className={clsx("flex px-10 gap-2.5 sm:gap-5 ml-auto items-center shrink-0", "row-start-1 col-start-1 xl:col-start-2")}
        style={{
          paddingTop: "var(--layout-pad)",
          paddingBottom: "var(--layout-pad)"
        }}
      >
        <LocaleSwitcher compact />
        <Sheet.Root>
          <Sheet.Trigger className="cursor-pointer group">
            {user.loading ? (
              <Span className="flex bg-background-surface size-10 rounded-full animate-pulse" />
            ) : (
              <Avatar
                src={user.account.avatar}
                alt={user.account.name}
                className="size-10 transition-all outline-transparent outline-0 outline-offset-2 group-data-[state=open]:outline-3 group-data-[state=open]:outline-background-highlight"
              />
            )}
          </Sheet.Trigger>
          <Sheet.Content title={t("modals.account.title")} description={t("modals.account.description")} closeButton side="right" />
        </Sheet.Root>
      </Container>
      <ScrollArea.Root>
        <ScrollArea.Viewport ref={scroller} className={clsx("row-start-2 col-start-1", "xl:col-start-2")}>
          <Container className="flex flex-col">{props.children}</Container>
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
