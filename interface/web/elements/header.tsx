// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { useEffect, useState } from "react";

import { Container, createElement, Portal, Span } from "@sptlco/design";

/**
 * A page header element.
 */
export const Header = createElement<typeof Container, { title?: string; description?: string }>((props, ref) => {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  return (
    (props.title || props.description) && (
      <Container {...props} ref={ref} className="flex flex-col px-10">
        {mounted && props.title && (
          <Portal container={document.getElementById("title")!}>
            <Span className="font-bold xl:text-foreground-quaternary xl:font-normal">{props.title}</Span>
          </Portal>
        )}
      </Container>
    )
  );
});
