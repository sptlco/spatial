// Copyright © Spatial Corporation. All rights reserved.

import { defineRouting } from "next-intl/routing";

/**
 * Locale routing configuration.
 */
export const routing = defineRouting({
  locales: ["de", "en-US", "es", "ja", "sw"],
  defaultLocale: "en-US"
});
