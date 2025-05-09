// Copyright © Spatial. All rights reserved.

import { Node, Page, Symbol } from "@spatial/elements";

/**
 * Create a new landing page element.
 * @returns A landing page element.
 */
export default function Spatial(): Node {
  return (
    <Page className="items-center justify-center">
      <Symbol className="h-1u" />
    </Page>
  );
}
