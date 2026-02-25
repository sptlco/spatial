// Copyright © Spatial Corporation. All rights reserved.

import createMDX from "@next/mdx";
import type { NextConfig } from "next";
import createNextIntlPlugin from "next-intl/plugin";

const withNextIntl = createNextIntlPlugin("./locales/request.ts");
const withMDX = createMDX({});

const nextConfig: NextConfig = {
  reactCompiler: true,
  allowedDevOrigins: ["s1.sptlco.com"],
  pageExtensions: ["js", "jsx", "md", "mdx", "ts", "tsx"],
  devIndicators: false,
  experimental: {
    externalDir: true,
    viewTransition: true
  }
};

export default withMDX(withNextIntl(nextConfig));
