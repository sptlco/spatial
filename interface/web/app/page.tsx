// Copyright © Spatial Corporation. All rights reserved.

import { Div, Logo } from "@sptlco/matter";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  return (
    <Div className="flex w-full h-screen items-center justify-center">
      <Logo mode="wordmark" className="h-6 md:h-12 fill-white" />
    </Div>
  );
}
