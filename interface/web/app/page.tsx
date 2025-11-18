// Copyright © Spatial Corporation. All rights reserved.

import { Image } from "@sptlco/matter";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  return (
    <div className="flex w-full h-screen items-center justify-center">
      <Image src="/symbol.svg" alt="Spatial Symbol" />
    </div>
  );
}
