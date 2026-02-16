// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { usePathname, useRouter } from "@/locales/navigation";
import { routing } from "@/locales/routing";
import { clsx } from "clsx";
import { useTransition } from "react";
import { Locale, useLocale, useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";

import { Button, Combobox, Icon, Span, Spinner } from "@sptlco/design";

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
    <Combobox.Root selection={locale} onSelect={change}>
      <Combobox.Trigger asChild>
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
      </Combobox.Trigger>
      <Combobox.Content>
        <Combobox.List>
          {routing.locales.map((locale, i) => (
            <Combobox.Item
              key={i}
              value={locale}
              label={t("name", { locale: locale.replace("-", "_") })}
              description={t("origin", { locale: locale.replace("-", "_") })}
            />
          ))}
        </Combobox.List>
      </Combobox.Content>
    </Combobox.Root>
  );
};
