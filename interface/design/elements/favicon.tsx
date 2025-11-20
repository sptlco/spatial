// Copyright © Spatial Corporation. All rights reserved.

import { createElement, resolve } from "..";

/**
 * Configurable options for a favicon.
 */
export type FaviconProps = {
  /**
   * The URL pointing to the icon file.
   */
  href: string;
};

/**
 * A small icon associated with a website, web page, or web application that helps users
 * visually identify it within browser tabs, bookmarks, shortcuts, and address bars.
 */
export const Favicon = createElement<"link", FaviconProps>((props, _) => {
  const href = resolve(props.href);

  return (
    <>
      <link {...props} rel="icon" href={href} type={props.type} sizes={props.sizes} />
      <link {...props} rel="apple-touch-icon" href={href} type={props.type} sizes={props.sizes} />
    </>
  );
});
