// Copyright © Spatial Corporation. All rights reserved.

import { AlertSeverity, ElementProps } from "..";

/**
 * Configurable options for an alert element.
 */
export type AlertProps = ElementProps & {
  /**
   * The severity of the alert.
   */
  severity?: AlertSeverity;
};
