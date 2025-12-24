// Copyright © Spatial Corporation. All rights reserved.

import type { NextConfig } from "next";
import createNextIntlPlugin from "next-intl/plugin";

const withNextIntl = createNextIntlPlugin();

const nextConfig: NextConfig = {
  basePath: process.env.NEXT_PUBLIC_BASE_URL,
  reactCompiler: true,
  allowedDevOrigins: ["dev.sptlco.com", "*.dev.sptlco.com"],
  experimental: {
    externalDir: true
  }
};

export default withNextIntl(nextConfig);
