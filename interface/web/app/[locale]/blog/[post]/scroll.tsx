// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { Fragment, useContext, useEffect, useState } from "react";

import { createElement, Icon, Portal, ScrollAreaContext } from "@sptlco/design";

export const ScrollIndicator = createElement<typeof Fragment>((props, _) => {
  const { scrollY } = useContext(ScrollAreaContext);

  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) {
    return null;
  }

  return (
    <Portal container={document.body}>
      <Icon
        {...props}
        symbol="arrow_downward"
        className={clsx(
          "pointer-events-none",
          "flex items-center justify-center size-10",
          "bg-input rounded-full",
          "fixed bottom-10 left-1/2 -translate-x-1/2",
          "transition-opacity duration-300",
          { "opacity-0": scrollY > 0 }
        )}
      />
    </Portal>
  );
});
