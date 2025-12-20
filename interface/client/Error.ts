// Copyright © Spatial Corporation. All rights reserved.

/**
 * An error that occurred during a request.
 */
export type Error = {
  /**
   * The time the error occurred.
   */
  time: number;

  /**
   * A trace identifier.
   */
  traceId: string;

  /**
   * The response's status code.
   */
  status: number;

  /**
   * A classifying error code.
   */
  code: string;

  /**
   * A message describing the error that occurred.
   */
  message: string;
};
