// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { createContext, useState } from "react";

import { createElement } from "..";

/**
 * Contextual data about the document.
 */
export const DocumentContext = createContext({
  className: "",
  setClassName: (_: string) => {}
});

/**
 * A document written in HTML.
 */
export const Html = createElement<"html">((props, ref) => {
  const [className, setClassName] = useState("");

  return (
    <DocumentContext.Provider value={{ className, setClassName }}>
      <html
        {...props}
        ref={ref}
        className={clsx("size-full overflow-hidden transition-all", "bg-background-base text-foreground-primary", className, props.className)}
      />
    </DocumentContext.Provider>
  );
});
