// Copyright © Spatial Corporation. All rights reserved.

import { ThemeProvider } from "@teispace/next-themes";
import { getTheme, getThemeScript } from "@teispace/next-themes/server";
import type { Metadata } from "next";
import { NextIntlClientProvider } from "next-intl";

import { Body, Head, Html, Script } from "@ux";

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

  const theme = await getTheme();
  const script = getThemeScript({
    attribute: "class",
    initialTheme: theme ?? undefined
  });

  return (
    <Html lang={locale} suppressHydrationWarning>
      <Head>
        <Script dangerouslySetInnerHTML={{ __html: script }} />
      </Head>
      <Body className="overflow-hidden">
        <NextIntlClientProvider>
          <ThemeProvider attribute="class" initialTheme={theme ?? undefined} defaultTheme="system" enableSystem disableTransitionOnChange noScript>
            {props.children}
          </ThemeProvider>
        </NextIntlClientProvider>
      </Body>
    </Html>
  );
}
