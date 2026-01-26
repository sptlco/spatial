// Copyright © Spatial Corporation. All rights reserved.

import { Error } from ".";

/**
 * A response that indicates failure due to an error.
 */
export type ErrorResponse = {
  /**
   * The error that occurred.
   */
  error: Error;
};
