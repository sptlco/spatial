// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Container, Link, Logo, Main } from "@sptlco/design";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  const links = [
    //{ href: "/blog", label: "Research" },
    //{ href: "/store", label: "Storefront" },
    { href: "/platform", label: "Platform" }
  ];
  return (
    <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] w-full h-screen gap-10">
      <Container className="col-start-3 row-start-1 place-self-end p-10"></Container>
      <Container className="col-span-full place-self-center flex flex-col gap-10 items-center justify-center">
        <Logo mode="wordmark" className="h-9 md:h-12 fill-white" />
        <Container className="flex flex-col md:flex-row items-center uppercase gap-5 md:gap-10 text-xs md:text-sm">
          {links.map((link, i) => (
            <Link key={i} href={link.href} className="text-hint! inline-flex items-center">
              {link.label}
            </Link>
          ))}
        </Container>
      </Container>
    </Main>
  );
}
