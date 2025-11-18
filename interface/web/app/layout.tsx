// Copyright © Spatial Corporation. All rights reserved.

import type { Metadata } from "next";
import { ThemeProvider } from "next-themes";
import { Favicon } from "@sptlco/matter";

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
        <ThemeProvider>{props.children}</ThemeProvider>
      </body>
    </html>
  );
}
