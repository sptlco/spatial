// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Response, Spatial } from "@sptlco/client";
import { createElement } from "@sptlco/design";
import { createContext, Fragment, useContext, useMemo } from "react";
import useSWR, { SWRResponse } from "swr";

/**
 * Contextual data related to the platform.
 */
export type PlatformContext = {
  __name: SWRResponse<Response<string>, any, any>;
  __version: SWRResponse<Response<string>, any, any>;
};

// Use isLoading: true so consumers correctly show a loading state
// before the ContextProvider has mounted and fetched.

const __default: PlatformContext = {
  __name: { isLoading: true, isValidating: false, data: undefined, error: undefined } as any,
  __version: { isLoading: true, isValidating: false, data: undefined, error: undefined } as any
};

const Context = createContext<PlatformContext>(__default);

/**
 * Provides the current platform context.
 */
export const ContextProvider = createElement<typeof Fragment>((props, _) => {
  const name = useSWR("name", Spatial.name);
  const version = useSWR("version", Spatial.version);

  return <Context.Provider {...props} value={{ __name: name, __version: version }} />;
});

/**
 * Get the current platform context.
 */
export const usePlatform = () => {
  const { __name, __version } = useContext(Context);

  return useMemo(
    () => ({
      __name,
      __version,
      name: (__name.data && !__name.data.error && __name.data.data) || "",
      version: (__version.data && !__version.data.error && __version.data.data) || ""
    }),

    // SWR objects are referentially stable between renders unless the response
    // actually changes, so this memo only recomputes when data truly updates.
    [__name, __version]
  );
};
