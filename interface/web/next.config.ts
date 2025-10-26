// Copyright © Spatial Corporation. All rights reserved.

import type { NextConfig } from "next";
import path from "path";

const nextConfig: NextConfig = {
    webpack(config) {
        config.resolve.alias["@sptlco/matter"] = path.resolve(__dirname, "../design");
        config.resolve.alias["@sptlco/matter/*"] = path.resolve(__dirname, "../design/*");

        return config;
    }
};

export default nextConfig;
