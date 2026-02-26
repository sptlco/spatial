// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { Container, createElement, Field, H2, Span } from "@sptlco/design";

export const Address = createElement<typeof Span>(() => {
  const response = useSWR("platform/logistics/trades/address", Spatial.address);
  const address = response.data && !response.data.error ? response.data.data : undefined;

  return (
    <Container className="flex w-full max-w-screen px-10">
      <Field
        type="text"
        readOnly
        inset={false}
        label={<H2 className="inline-flex text-2xl font-extrabold">Address</H2>}
        className="text-sm xl:text-base min-w-0"
        containerClassName="py-5 xl:py-10 xl:gap-10 xl:flex-row xl:items-center max-w-md min-w-0"
        value={address}
      />
    </Container>
  );
});
