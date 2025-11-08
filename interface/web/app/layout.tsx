// Copyright © Spatial Corporation. All rights reserved.

import type { Metadata } from "next";
import { ThemeProvider } from "next-themes";
import { Suspense } from "react";

import { Favicon } from "@spatial/components";

import "@sptlco/matter";

export const metadata: Metadata = {
  title: "Spatial",
  description: "Leading industrial research and development."
};

export default function Layout(props: { children: React.ReactNode }) {
  return (
    <html className="h-full" suppressHydrationWarning>
      <head>
        <Favicon href="/favicon.ico" />
      </head>
      <body className="h-full bg-background-primary text-foreground-primary text-base font-regular">
        <Suspense>
          <ThemeProvider>{props.children}</ThemeProvider>
        </Suspense>
      </body>
    </html>
  );
}
