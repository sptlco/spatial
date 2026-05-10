// Copyright © Spatial Corporation. All rights reserved.

import axios, { AxiosRequestConfig } from "axios";
import cookies from "js-cookie";

import { NativeError, SESSION_COOKIE_NAME } from ".";

const client = axios.create({
  baseURL: process.env.NEXT_PUBLIC_SERVER_ENDPOINT
});

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
  protected post = async <R>(path: string, body?: any, config?: AxiosRequestConfig) => this.fetch<R>(path, "POST", body, config);

  /**
   * Send a GET request to the server.
   * @param path The path to send the request to.
   * @returns A response from the server.
   */
  protected get = async <R>(path: string, config?: AxiosRequestConfig) => this.fetch<R>(path, "GET", config);

  /**
   * Send a PATCH request to the server.
   * @param path The path to send the request to.
   * @param body The request's body.
   * @returns A response from the server.
   */
  protected patch = async <R>(path: string, body?: any, config?: AxiosRequestConfig) => this.fetch<R>(path, "PATCH", body, config);

  /**
   * Send a DELETE request to the server.
   * @param path The path to send the request to.
   * @param body The request's body.
   * @returns A response from the server.
   */
  protected delete = async <R>(path: string, body?: any, config?: AxiosRequestConfig) => this.fetch<R>(path, "DELETE", body, config);

  private fetch = async <T>(path: string, method: string, body?: any, config?: AxiosRequestConfig): Promise<T> => {
    try {
      let request: AxiosRequestConfig = {
        url: path,
        method: method,
        data: body,
        ...config
      };

      const session = cookies.get(SESSION_COOKIE_NAME);

      if (session) {
        request = {
          ...request,
          headers: {
            ...request.headers,
            Authorization: `Bearer ${session}`
          }
        };
      }

      return (await client(request)).data;
    } catch (error: any) {
      if (axios.isAxiosError(error) && error.response) {
        throw new NativeError(error.response.data);
      }

      throw error;
    }
  };
}
