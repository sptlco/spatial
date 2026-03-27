// Copyright © Spatial Corporation. All rights reserved.

/**
 * Indicates the current status of a {@link Parcel}.
 */
export type ParcelStatus = "Processed" | "Shipped" | "Moving" | "Delivered";

/**
 * An iterable array of parcel statuses.
 */
export const PARCEL_STATUSES: ParcelStatus[] = ["Processed", "Shipped", "Moving", "Delivered"];
