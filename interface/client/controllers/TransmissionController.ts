// Copyright © Spatial Corporation. All rights reserved.

import { Controller } from "..";

/**
 * A {@link Controller} for transmission functions.
 */
export class TransmissionController extends Controller {
  /**
   * Stream a transmission.
   * @param transmission The transmission to stream.
   * @param passphrase The transmission's passphrase.
   * @returns A transmission stream.
   */
  public stream = (transmission: string, passphrase: string) =>
    this.post<Blob>(`transmissions/${transmission}`, passphrase, { responseType: "blob", headers: { "Content-Type": "application/json" } });
}
