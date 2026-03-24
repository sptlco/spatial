// Copyright © Spatial Corporation. All rights reserved.

import { CreateShipmentOptions, Shipment } from "@sptlco/data";
import { Controller } from "..";

export class ShipmentController extends Controller {
  /**
   * Create a new shipment.
   * @param options Configurable options for the shipment.
   * @returns A shipment.
   */
  public create = async (options: CreateShipmentOptions) => {
    return this.post<Shipment>("shipments", options);
  };

  /**
   * List all shipments.
   * @returns A list of shipments.
   */
  public list = async () => {
    return this.get<Shipment[]>("shipments");
  };

  /**
   * Update a shipment.
   * @param shipment The shipment to update.
   * @returns The updated shipment.
   */
  public update = async (shipment: Shipment) => {
    return this.patch<Shipment>("shipments", shipment);
  };

  /**
   * Delete a shipment.
   * @param id The ID of the shipment to delete.
   */
  public del = async (id: string) => {
    return this.delete(`shipments/${id}`);
  };
}
