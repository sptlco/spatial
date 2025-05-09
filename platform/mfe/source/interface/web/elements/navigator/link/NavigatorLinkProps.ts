// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a navigator link element.
 */
export type NavigatorLinkProps = ElementProps & {
  /**
   * Whether or not the link is active.
   */
  active?: boolean;

  /**
   * An optional select event handler.
   * @param event The selection event.
   */
  onSelect?: (event: Event) => void;

  /**
   * An optional hypertext reference.
   */
  href?: string;
};
