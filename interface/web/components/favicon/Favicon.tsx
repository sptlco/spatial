// Copyright © Spatial Corporation. All rights reserved.

import { FC, ReactNode } from "react";

type FaviconProps = {
  href: string;
  type?: string;
  sizes?: string;
};

/**
 * Create a new favicon component.
 * @param param0 Configurable options for the component.
 * @returns A favicon component.
 */
export const Favicon: FC<FaviconProps> = ({ href, type = "image/x-icon", sizes }: FaviconProps): ReactNode => {
  let base = process.env.NEXT_PUBLIC_BASE_URL;

  href = base ? `${base.replace(/\/$/, "")}/${href.replace(/^\//, "")}` : href;

  return (
    <>
      <link rel="icon" href={href} type={type} sizes={sizes} />
      <link rel="apple-touch-icon" href={href} sizes={sizes} />
    </>
  );
};
