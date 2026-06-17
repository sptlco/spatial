// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

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
  return (
    <>
      <link {...props} rel="icon" href={props.href} type={props.type} sizes={props.sizes} />
      <link {...props} rel="apple-touch-icon" href={props.href} type={props.type} sizes={props.sizes} />
    </>
  );
});
