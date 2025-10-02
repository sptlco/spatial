// Copyright © Spatial Corporation. All rights reserved.

import { Layout, Symbol } from "@spatial/ux";

/**
 * Create a new authentication page.
 * @returns An authentication page.
 */
export default function Page() {
  return (
    <Layout className="flex flex-col space-y-12 w-full min-h-screen items-center justify-center">
      <Symbol className="h-12 fill-foreground-primary" />
    </Layout>
  );
}
