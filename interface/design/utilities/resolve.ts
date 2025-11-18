// Copyright © Spatial Corporation. All rights reserved.

/**
 * Resolve a static file path.
 * @param path A static file path to resolve.
 * @returns A resolved file path.
 */
export const resolve = (path: string): string => {
  let base = process.env.NEXT_PUBLIC_BASE_URL;
  return base ? `${base.replace(/\/$/, "")}/${path.replace(/^\//, "")}` : path;
};
