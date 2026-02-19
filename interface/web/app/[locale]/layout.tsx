// Copyright © Spatial Corporation. All rights reserved.

import { Service, User } from "@/elements";
import { ContextProvider } from "@/utilities";
import { Body, Html, Toaster, Tooltip } from "@sptlco/design";
import type { Metadata } from "next";
import { NextIntlClientProvider } from "next-intl";

import "./global.css";

/**
 * Top-level application metadata for improved SEO and web shareability.
 */
export const metadata: Metadata = {
  title: "Spatial",
  description: "Leading industrial research and development."
};

/**
 * The top-most layout defining shared UI for all paths under the root directory.
 * @param props Configurable options for the layout.
 * @returns A layout element.
 */
export default async function Layout(props: { children: React.ReactNode; params: Promise<{ locale: string }> }) {
  const { locale } = await props.params;

  return (
    <Html lang={locale} suppressHydrationWarning>
      <Body className="overflow-hidden">
        <Tooltip.Provider>
          <NextIntlClientProvider>
            <ContextProvider>{props.children}</ContextProvider>
          </NextIntlClientProvider>
        </Tooltip.Provider>
        <Toaster />
        <Service />
        <User />
      </Body>
    </Html>
  );
}
