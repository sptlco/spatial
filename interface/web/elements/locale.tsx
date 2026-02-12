// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Button, Container, Dropdown, Icon, Span, Spinner } from "@sptlco/design";
import { usePathname, useRouter } from "@/locales/navigation";
import { routing } from "@/locales/routing";
import { clsx } from "clsx";
import { useTransition } from "react";
import { Locale, useLocale, useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";

/**
 * A dropdown menu that allows the user to change locales.
 * @returns A locale switcher component.
 */
export const LocaleSwitcher = ({ compact = false }: { compact?: boolean }) => {
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

  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>
        <Button
          intent="ghost"
          className={clsx("group data-[state=open]:bg-button-ghost-active", { "px-2! sm:px-4! rounded-lg sm:rounded-full": compact })}
        >
          {pending ? (
            <Span className="inline-flex size-6 items-center justify-center">
              <Spinner className="size-4 text-foreground-secondary" />
            </Span>
          ) : (
            <>
              <Icon symbol="language" className="group-data-[state=open]:fill" />
              <Span className={compact ? "hidden sm:inline" : undefined}>{t("label")}</Span>
            </>
          )}
        </Button>
      </Dropdown.Trigger>
      <Dropdown.Portal>
        <Dropdown.Content asChild>
          <Dropdown.RadioGroup value={locale} onValueChange={change}>
            {routing.locales.map((locale, i) => (
              <Dropdown.RadioItem key={i} value={locale} className="flex items-center gap-10">
                <Container className="flex flex-col grow">
                  <Span>{t("name", { locale: locale.replace("-", "_") })}</Span>
                  <Span className="text-sm text-foreground-secondary">{t("origin", { locale: locale.replace("-", "_") })}</Span>
                </Container>
                <Dropdown.ItemIndicator className="flex items-center justify-center">
                  <Icon symbol="check" size={20} />
                </Dropdown.ItemIndicator>
              </Dropdown.RadioItem>
            ))}
          </Dropdown.RadioGroup>
        </Dropdown.Content>
      </Dropdown.Portal>
    </Dropdown.Root>
  );
};
