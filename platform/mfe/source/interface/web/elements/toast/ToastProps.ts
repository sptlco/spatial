// Copyright © Spatial. All rights reserved.

import {
  ElementProps,
  IconVariant,
  Node,
  ToastActionProps,
  ToastSeverity,
} from "..";

/**
 * Configurable options for a toast element.
 */
export type ToastProps = ElementProps & {
  /**
   * The toast's type.
   */
  type?: "background" | "foreground";

  /**
   * The severity of the toast.
   */
  severity?: ToastSeverity;

  /**
   * An optional icon to render.
   */
  icon?: IconVariant;

  /**
   * An optional title.
   */
  title?: Node;

  /**
   * The toast's description.
   */
  description: Node;

  /**
   * An optional action.
   */
  action?: ToastActionProps;

  /**
   * An optional duration.
   */
  duration?: number;

  /**
   * Whether or not the toast is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the toast is open.
   */
  onChange?: (open: boolean) => void;
};
