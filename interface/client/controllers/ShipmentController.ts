// Copyright © Spatial Corporation. All rights reserved.

import { CreateShipmentOptions, Parcel, Shipment } from "@sptlco/data";
import { Controller } from "..";

/**
 * A {@link Controller} for {@link Shipment} functions.
 */
export class ShipmentController extends Controller {
  /**
   * Create a new {@link Shipment}.
   * @param options Configurable options for the {@link Shipment}.
   * @returns A {@link Shipment}.
   */
  public create = (options: CreateShipmentOptions) => this.post<Shipment>("shipments", options);

  /**
   * Find a {@link Shipment}.
   * @param shipment A {@link Shipment} identifier.
   * @returns All packages with the {@link Shipment} identifier.
   */
  public find = (shipment: string) => this.get<Shipment>(`shipments/${shipment}`);

  /**
   * List all shipments.
   * @returns A list of shipments.
   */
  public list = () => this.get<Shipment[]>("shipments");

  /**
   * Update a {@link Parcel}.
   * @param parcel The {@link Parcel} to update.
   * @returns The updated {@link Parcel}.
   */
  public updateParcel = (parcel: Parcel) => this.patch<Parcel>(`shipments/parcels/${parcel.id}`, parcel);

  /**
   * Delete a {@link Shipment}.
   * @param shipment The {@link Shipment} to delete.
   */
  public del = (shipment: string) => this.delete(`shipments/${shipment}`);

  /**
   * Delete a {@link Parcel}.
   * @param parcel The {@link Parcel} to delete.
   */
  public deleteParcel = (parcel: string) => this.delete(`shipments/parcels/${parcel}`);
}
