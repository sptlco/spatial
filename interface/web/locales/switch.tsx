// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Locale, useLocale, useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";
import { usePathname, useRouter } from "./navigation";
import { useTransition } from "react";
import { Button, Container, Dropdown, Icon, Span, Spinner } from "@sptlco/design";
import { routing } from "./routing";

/**
 * A dropdown menu that allows the user to change locales.
 * @returns A locale switcher component.
 */
export const LocaleSwitcher = () => {
  const locale = useLocale();
  const params = useSearchParams();
  const pathname = usePathname();
  const router = useRouter();

  const [pending, startTransition] = useTransition();

  const t = useTranslations("locale");

  const change = (next: string) => {
    startTransition(() => {
      router.replace({ pathname, query: Object.fromEntries(params.entries()) }, { locale: next as Locale });
    });
  };

  if (pending) {
    return (
      <>
        <Container className="z-50 fixed top-0 left-0 inset-0 bg-background-base/20 backdrop-blur w-screen h-screen animate-in fade-in-0 duration-500" />
        <Spinner className="z-51 size-4" />
      </>
    );
  }

  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>
        <Button intent="ghost" shape="pill" className="data-[state=open]:bg-button-ghost-active">
          <Icon symbol="language" />
          <Span>{t("label")}</Span>
        </Button>
      </Dropdown.Trigger>
      <Dropdown.Portal>
        <Dropdown.Content className="bg-background-surface rounded-lg p-4 w-screen max-w-48 sm:max-w-3xs" sideOffset={10} collisionPadding={40}>
          <Dropdown.RadioGroup value={locale} onValueChange={change} className="flex flex-col gap-2">
            {routing.locales.map((locale, i) => (
              <Dropdown.RadioItem
                key={i}
                value={locale}
                onSelect={(e) => e.preventDefault()}
                className="py-2 px-4 cursor-pointer transition-all data-highlighted:bg-button-ghost-active rounded-lg flex items-center"
              >
                <Container className="flex flex-col grow">
                  <Span className="text-sm font-bold">{t("name", { locale: locale.replace("-", "_") })}</Span>
                  <Span className="text-xs">{t("origin", { locale: locale.replace("-", "_") })}</Span>
                </Container>
                <Dropdown.ItemIndicator className="text-xs flex items-center justify-center">
                  <Icon symbol="check" fill />
                </Dropdown.ItemIndicator>
              </Dropdown.RadioItem>
            ))}
          </Dropdown.RadioGroup>
        </Dropdown.Content>
      </Dropdown.Portal>
    </Dropdown.Root>
  );
};
