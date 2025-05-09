// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for a toast provider element.
 */
export type ToastProviderProps = ElementProps & {
  /**
   * The duration of toast notifications.
   */
  duration?: number;

  /**
   * A label for notifications.
   */
  label?: string;

  /**
   * The direction to swipe toasts to dismiss them.
   */
  swipeDirection?: "right" | "left" | "up" | "down";

  /**
   * The distance in pixels the toast must be swiped
   * before it is dismissed.
   */
  swipeThreshold?: number;
};
