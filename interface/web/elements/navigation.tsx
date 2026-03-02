// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Logo } from "@sptlco/design";

export const Navigation = createElement<"nav">((props, ref) => {
  return (
    <nav {...props} ref={ref} className="flex flex-col items-center">
      <Container className="flex items-center justify-between gap-10 p-10 w-full xl:max-w-7xl">
        <Logo mode="symbol" className="h-6" />
      </Container>
    </nav>
  );
});
