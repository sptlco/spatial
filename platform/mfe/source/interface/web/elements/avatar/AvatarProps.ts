// Copyright © Spatial. All rights reserved.

import { AvatarMode, ElementProps } from "..";

/**
 * Configurable options for an avatar element.
 */
export type AvatarProps = ElementProps & {
  /**
   * The avatar's mode.
   */
  mode: AvatarMode;

  /**
   * Optional text to abbreviate.
   */
  text?: string;

  /**
   * An optional image URL.
   */
  url?: string;

  /**
   * Alternate text to display.
   */
  alt: string;
};
