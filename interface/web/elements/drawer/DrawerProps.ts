// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a drawer element.
 */
export type DrawerProps = ElementProps & {
  /**
   * Whether or not the drawer is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the drawer is open.
   */
  onOpenChange?: (open: boolean) => void;

  /**
   * An optional list of snap points.
   */
  snapPoints?: (number | string)[];

  /**
   * The drawer's active snap point.
   */
  activeSnapPoint?: number | string | null;

  /**
   * An optional setter for the active snap point.
   */
  setActiveSnapPoint?: (snapPoint: number | string | null) => void;

  /**
   * Whether or not the drawer should snap to each point sequentially,
   * regardless of the scroll velocity.
   */
  snapToSequentialPoint?: boolean;

  /**
   * Whether or not to displat the drawer as a modal.
   */
  modal?: boolean;
};
