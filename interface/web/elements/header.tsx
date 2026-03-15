// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Fragment, useEffect, useState } from "react";

import { createElement, Portal, Span } from "@sptlco/design";

/**
 * A page header element.
 */
export const Header = createElement<typeof Fragment, { title?: string }>((props, ref) => {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  return (
    mounted &&
    props.title && (
      <Portal {...props} ref={ref} container={document.getElementById("title")!}>
        <Span className="xl:text-foreground-quaternary font-semibold xl:font-normal text-sm xl:text-base">{props.title}</Span>
      </Portal>
    )
  );
});
