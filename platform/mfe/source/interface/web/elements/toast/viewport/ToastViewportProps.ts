// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a toast viewport element.
 */
export type ToastViewportProps = ElementProps & {
  /**
   * An optional label for the viewport.
   */
  label?: string;
};
