// Copyright © Spatial Corporation. All rights reserved.

import type { NextConfig } from "next";
import createNextIntlPlugin from "next-intl/plugin";

const withNextIntl = createNextIntlPlugin("./locales/request.ts");

const nextConfig: NextConfig = {
  reactCompiler: true,
  allowedDevOrigins: ["s1.sptlco.com"],
  experimental: {
    externalDir: true,
    viewTransition: true
  }
};

export default withNextIntl(nextConfig);
