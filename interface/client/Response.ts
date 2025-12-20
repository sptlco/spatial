// Copyright © Spatial Corporation. All rights reserved.

import { ErrorResponse, Payload } from ".";

/**
 * A response from the server.
 */
export type Response<T> = Payload<T> | ErrorResponse;
