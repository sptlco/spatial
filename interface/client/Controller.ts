// Copyright © Spatial Corporation. All rights reserved.

import { Response, SESSION_COOKIE_NAME } from ".";
import axios, { AxiosRequestConfig } from "axios";
import cookies from "js-cookie";

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
      let config: AxiosRequestConfig = {
        url: path,
        method: method,
        data: body
      };

      const session = cookies.get(SESSION_COOKIE_NAME);

      if (session) {
        config = {
          ...config,
          headers: {
            Authorization: `Bearer ${session}`
          }
        };
      }

      const response = await client(config);

      return { data: response.data };
    } catch (error: any) {
      if (axios.isAxiosError(error) && error.response) {
        return { error: error.response.data };
      }

      throw error;
    }
  };
}
