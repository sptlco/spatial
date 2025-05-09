// Copyright © Spatial. All rights reserved.

import { AriaRole, CSSProperties, JSXElementConstructor, Ref } from "react";

import { Node } from ".";

/**
 * Configurable options for an element.
 */
export type ElementProps = {
  /**
   * A reference.
   */
  ref?: Ref<any>;

  /**
   * An optional class to apply to the element.
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

  /**
   * The element's role.
   */
  role?: AriaRole;
};
