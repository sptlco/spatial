// Copyright © Spatial Corporation. All rights reserved.

/**
 * An error that occurred during a request.
 */
export type ErrorResponse = {
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

/**
 * Thrown when the server returns an {@link ErrorResponse}.
 */
export class NativeError extends Error {
  public readonly response: ErrorResponse;

  constructor(response: ErrorResponse) {
    super(response.message);
    this.response = response;
  }
}
