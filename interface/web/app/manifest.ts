// Copyright © Spatial Corporation. All rights reserved.

import { MetadataRoute } from "next";

export default function manifest(): MetadataRoute.Manifest {
  return {
    name: "Spatial",
    short_name: "Spatial",
    start_url: "/platform",
    display: "standalone",
    background_color: "#0a0a0a",
    theme_color: "#fff",
    icons: [
      {
        src: "/icon.png",
        sizes: "512x512",
        type: "image/png"
      },
      {
        src: "/icon-maskable.png",
        sizes: "512x512",
        type: "image/png",
        purpose: "maskable"
      }
    ]
  };
}
