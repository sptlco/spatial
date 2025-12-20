// Copyright © Spatial Corporation. All rights reserved.

/**
 * A response that contains a message payload.
 */
export type Payload<T> = {
  /**
   * The error that occurred.
   */
  error?: never;

  /**
   * The requested payload.
   */
  data: T;
};
