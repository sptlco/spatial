// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Container, Icon, Link, Logo, Main, Span } from "@sptlco/design";

/**
 * Create a new landing page component.
 * @returns A landing page component.
 */
export default function Page() {
  return (
    <Main className="grid grid-cols-[1fr_auto_1fr] grid-rows-[auto_1fr_auto] w-full h-screen gap-10">
      <Container className="col-start-3 row-start-1 place-self-end p-10"></Container>
      <Container className="col-span-full place-self-center flex flex-col gap-10 items-center justify-center">
        <Logo mode="wordmark" className="h-9 md:h-12 fill-white" />
        <Container className="flex flex-col md:flex-row items-center uppercase text-hint gap-5 md:gap-10 text-xs md:text-sm">
          <Link href="/blog" className="text-inherit! inline-flex items-center">
            <Span>Research</Span>
            <Icon symbol="arrow_outward" size={16} className="flex font-light" />
          </Link>
          <Link href="/blog" className="text-inherit! inline-flex items-center">
            <Span>Conference</Span>
            <Icon symbol="arrow_outward" size={16} className="flex font-light" />
          </Link>
          <Link href="/store" className="text-inherit! inline-flex items-center">
            <Span>Storefront</Span>
            <Icon symbol="arrow_outward" size={16} className="flex font-light" />
          </Link>
          <Link href="/platform" className="text-inherit! inline-flex items-center">
            <Span>Platform</Span>
            <Icon symbol="arrow_outward" size={16} className="flex font-light" />
          </Link>
        </Container>
      </Container>
    </Main>
  );
}
