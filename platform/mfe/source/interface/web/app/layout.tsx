// Copyright © Spatial. All rights reserved.

import { Metadata } from "next";
import Head from "next/head";
import { Analytics } from "@vercel/analytics/react";

import { ToastProvider } from "@spatial/compounds";
import { Body, Html, Node, TooltipProvider } from "@spatial/elements";

import "@spatial/design";

/**
 * Configurable metadata for all pages.
 */
export const metadata: Metadata = {
  title: "Spatial",
  description: "Leading industrial research and development",
};

/**
 * Create a new root layout element.
 * @param props Configurable options for the element.
 * @returns A root layout element.
 */
export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}): Node {
  return (
    <Html>
      <Head>
        <meta charSet="UTF-8" />
      </Head>
      <Body>
        <TooltipProvider>{children}</TooltipProvider>
        <ToastProvider />
        <Analytics />
      </Body>
    </Html>
  );
}
