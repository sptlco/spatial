// Copyright © Spatial Corporation. All rights reserved.

import { H1 } from "@sptlco/design";
import { ViewTransition } from "react";

export default function Page() {
  return (
    <ViewTransition>
      <H1 className="text-4xl font-bold">Users</H1>
    </ViewTransition>
  );
}
