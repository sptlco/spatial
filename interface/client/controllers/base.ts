// Copyright © Spatial Corporation. All rights reserved.

import axios from "axios";

const client = axios.create({
  baseURL: process.env.NEXT_PUBLIC_SERVER_ENDPOINT
});

/**
 * A response from the server.
 */
export type Response<T> = Payload<T> | ErrorResponse;

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

/**
 * A response that indicates failure due to an error.
 */
export type ErrorResponse = {
  /**
   * The error that occurred.
   */
  error: Error;
};

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

/**
 * A mechanism that enables HTTP communication with the server.
 */
export class Controller {
  /**
   * Send a POST request to the server.
   * @param path The path to send the request to.
   * @param body The request's body.
   * @returns A response from the server.
   */
  protected post = async <R>(path: string, body: any) => this.fetch<R>(path, "POST", body);

  /**
   * Send a GET request to the server.
   * @param path The path to send the request to.
   * @returns A response from the server.
   */
  protected get = async <R>(path: string) => this.fetch<R>(path, "GET");

  /**
   * Send a PATCH request to the server.
   * @param path The path to send the request to.
   * @param body The request's body.
   * @returns A response from the server.
   */
  protected patch = async <R>(path: string, body: any) => this.fetch<R>(path, "PATCH", body);

  /**
   * Send a DELETE request to the server.
   * @param path The path to send the request to.
   * @param body The request's body.
   * @returns A response from the server.
   */
  protected delete = async <R>(path: string, body: any) => this.fetch<R>(path, "DELETE", body);

  private fetch = async <T>(path: string, method: string, body?: any): Promise<Response<T>> => {
    try {
      const response = await client({
        url: path,
        method: method,
        data: body
      });

      return { data: response.data };
    } catch (error: any) {
      if (axios.isAxiosError(error) && error.response) {
        return { error: error.response.data };
      }

      throw error;
    }
  };
}
