// Copyright © Spatial Corporation. All rights reserved.

import type { Metadata } from "next";
import { ThemeProvider } from "next-themes";
import { PropsWithChildren } from "react";

import "@spatial/ux";

export const metadata: Metadata = {
  title: "Spatial",
  description: "Leading industrial research and development."
};

export default function RootLayout({ children }: PropsWithChildren) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body className="bg-background-primary text-foreground-primary text-base font-regular">
        <ThemeProvider>{children}</ThemeProvider>
      </body>
    </html>
  );
}
