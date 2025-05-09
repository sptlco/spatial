import type { NextConfig } from "next";
import createMDX from "@next/mdx";

const config: NextConfig = {
  pageExtensions: ["js", "jsx", "md", "mdx", "ts", "tsx"],
  experimental: {
    mdxRs: true,
  },
  eslint: {
    ignoreDuringBuilds: true,
  },
  async redirects() {
    return [
      {
        source: "/",
        destination: "/conference",
        permanent: true,
      },
    ];
  },
};

const withMDX = createMDX({});

export default withMDX(config);
