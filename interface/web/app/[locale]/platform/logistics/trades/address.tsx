// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import useSWR from "swr";

import { createElement, Field, H2, Icon, Span } from "@sptlco/design";

export const Address = createElement<typeof Span>(() => {
  const response = useSWR("platform/logistics/trades/address", Spatial.address);
  const address = response.data && !response.data.error ? response.data.data : undefined;

  return (
    <Field
      type="text"
      readOnly
      inset={false}
      label={<H2 className="inline-flex text-2xl font-extrabold">Address</H2>}
      className="bg-transparent"
      containerClassName="ml-10 py-10 gap-10 w-full flex-row items-center max-w-xl"
      value={address}
    />
  );
});
