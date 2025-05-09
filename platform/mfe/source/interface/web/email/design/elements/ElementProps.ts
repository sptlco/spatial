// Copyright © Spatial. All rights reserved.

import { CSSProperties } from "react";

import { Node } from ".";

/**
 * Configurable options for an element.
 */
export type ElementProps = {
  /**
   * An optional class name.
   */
  className?: string;

  /**
   * Optional styles for the element.
   */
  style?: CSSProperties;

  /**
   * Optional children of the element.
   */
  children?: Node;
};
