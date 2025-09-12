// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a pagination link element.
 */
export type PaginationLinkProps = ElementProps & {
  /**
   * Whether or not the link is active.
   */
  active?: boolean;

  /**
   * A hypertextual reference.
   */
  href?: string;
};
