// Copyright © Spatial Corporation. All rights reserved.

import type { Metadata } from "next";
import { ThemeProvider } from "next-themes";

import "@spatial/ux";

export const metadata: Metadata = {
  title: "Spatial",
  description: "Leading industrial research and development."
};

export default function Layout(props: { children: React.ReactNode }) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className="bg-background-primary text-foreground-primary text-base font-regular">
        <ThemeProvider>{props.children}</ThemeProvider>
      </body>
    </html>
  );
}
