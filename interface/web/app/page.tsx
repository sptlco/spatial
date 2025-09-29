// Copyright © Spatial Corporation. All rights reserved.

import Image from "next/image";

export default function Page() {
  return (
    <div className="flex w-full h-screen items-center justify-center">
      <Image className="dark:invert" src="/symbol.svg" alt="Spatial Symbol" width={96} height={64} priority />
    </div>
  );
}
