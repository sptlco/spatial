// Copyright © Spatial Corporation. All rights reserved.

/**
 * Delay execution for a number of milliseconds.
 * @param ms The number of milliseconds to wait for.
 * @returns A promise.
 */
export const delay = (ms: number) => new Promise((resolve) => setTimeout(resolve, ms));
