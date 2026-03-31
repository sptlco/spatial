// Copyright © Spatial Corporation. All rights reserved.

import { Parcel } from "@sptlco/data";
import { Controller } from "..";

export class ParcelController extends Controller {
  /**
   * Update a parcel.
   * @param shipment The parcel to update.
   * @returns The updated parcel.
   */
  public update = (parcel: Parcel) => this.patch<Parcel>("shipments/parcels", parcel);
}
