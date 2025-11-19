// Copyright © Spatial Corporation. All rights reserved.

import { Favicon } from "@sptlco/matter";
import type { Metadata } from "next";

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
export default function Layout(props: { children: React.ReactNode }) {
  return (
    <html className="h-full bg-black" suppressHydrationWarning>
      <head>
        <Favicon href="/assets/favicon.ico" />
      </head>
      <body className="h-full text-base font-regular">{props.children}</body>
    </html>
  );
}
