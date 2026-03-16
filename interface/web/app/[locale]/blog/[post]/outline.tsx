// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { MouseEvent, useEffect, useState } from "react";

import { Container, createElement, Link, Separator, Span, UL } from "@sptlco/design";

/**
 * A visual hierarchy of the post.
 */
export const Outline = createElement<typeof Container>((props, ref) => {
  const [headers, setHeaders] = useState<HTMLHeadingElement[]>([]);

  function slug(children: React.ReactNode): string | undefined {
    if (typeof children !== "string") {
      return undefined;
    }

    return children
      .toLowerCase()
      .replace(/\s+/g, "-")
      .replace(/[^\w-]/g, "");
  }

  useEffect(() => {
    let array: HTMLHeadingElement[] = [];

    document.querySelectorAll("h2").forEach((header) => array.push(header));

    setHeaders(array);
  }, []);

  const scroll = (header: HTMLHeadingElement, e: MouseEvent<HTMLAnchorElement, globalThis.MouseEvent>) => {
    e.preventDefault();
    header.scrollIntoView({ behavior: "smooth" });
  };

  return (
    headers.length > 0 && (
      <>
        <Container {...props} ref={ref} className={clsx("flex flex-col gap-5", props.className)}>
          <Span className="text-foreground-secondary">On this page</Span>
          <UL className="flex flex-col gap-2 font-semibold">
            {headers.map((header, i) => {
              return (
                <Link key={i} href={`#${slug(header.innerHTML)}`} onClick={(e) => scroll(header, e)}>
                  {header.innerHTML}
                </Link>
              );
            })}
          </UL>
        </Container>
        <Separator orientation="horizontal" className="w-full h-px bg-line-base" />
      </>
    )
  );
});
