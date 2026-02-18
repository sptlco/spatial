// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Response, Spatial } from "@sptlco/client";
import { createElement } from "@sptlco/design";
import { createContext, Fragment, useContext as hook } from "react";
import useSWR, { SWRResponse } from "swr";

/**
 * Contextual data related to the platform.
 */
export type PlatformContext = {
  __name: SWRResponse<Response<string>, any, any>;
  __version: SWRResponse<Response<string>, any, any>;
};

const __default: PlatformContext = {
  __name: {} as any,
  __version: {} as any
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
  const context = hook(Context);

  return {
    ...context,
    name: (context.__name.data && !context.__name.data.error && context.__name.data.data) || "",
    version: (context.__version.data && !context.__version.data.error && context.__version.data.data) || ""
  };
};
