// Copyright © Spatial Corporation. All rights reserved.

import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  basePath: process.env.NEXT_PUBLIC_BASE_URL,
  experimental: {
    externalDir: true
  }
};

export default nextConfig;
