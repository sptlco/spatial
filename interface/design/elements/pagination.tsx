// Copyright © Spatial Corporation. All rights reserved.

import { createElement, LI, Link, Nav, Span, UL } from "..";

/**
 * Pagination with page navigation, next and previous links.
 */
export const Pagination = {
  /**
   * Contains all the parts of a pagination.
   */
  Root: createElement<"nav">((props, ref) => <Nav {...props} ref={ref} />),

  /**
   * Renders a list of pagination items.
   */
  Content: createElement<"ul">((props, ref) => <UL {...props} ref={ref} />),

  /**
   * Renders part of the pagination menu.
   */
  Item: createElement<"li">((props, ref) => <LI {...props} ref={ref} />),

  /**
   * Navigates to another page.
   */
  Link: createElement<typeof Link>((props, ref) => <Link {...props} ref={ref} />),

  /**
   * Navigates to the previous page.
   */
  Previous: createElement<typeof Link>((props, ref) => <Link {...props} ref={ref} />),

  /**
   * Navigates to the next page.
   */
  Next: createElement<typeof Link>((props, ref) => <Link {...props} ref={ref} />),

  /**
   * Signals there are more pages available.
   */
  Ellipsis: createElement<"span">((props, ref) => <Span {...props} ref={ref} />)
};
