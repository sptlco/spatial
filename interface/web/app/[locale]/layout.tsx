// Copyright © Spatial Corporation. All rights reserved.

import { User } from "@/elements";
import { Body, Favicon, Head, Html, Toaster } from "@sptlco/design";
import { clsx } from "clsx";
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
      <Head>
        <Favicon href="/assets/favicon.ico" />
      </Head>
      <Body>
        <NextIntlClientProvider>{props.children}</NextIntlClientProvider>
        <Toaster />
        <User />
      </Body>
    </Html>
  );
}
