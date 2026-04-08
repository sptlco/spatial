// Copyright © Spatial Corporation. All rights reserved.

/**
 * An object stored in the database.
 */
export type Schema<S> = Resource & S;

/**
 * An object stored in the database.
 */
export type Resource = {
  /**
   * The object's unique identifier.
   */
  id: string;

  /**
   * The time the object was created.
   */
  created: number;

  /**
   * Arbitrary key-value pairs related to the object.
   */
  metadata: Record<string, string>;
};
