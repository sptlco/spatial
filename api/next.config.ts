import type { NextConfig } from "next";

const config: NextConfig = {
  pageExtensions: ["js", "jsx", "ts", "tsx"],
  eslint: {
    ignoreDuringBuilds: true,
  }
};

export default config;
