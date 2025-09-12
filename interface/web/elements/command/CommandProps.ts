// Copyright © Spatial Corporation. All rights reserved.

import { ElementProps } from "..";

/**
 * Configurable options for a command element.
 */
export type CommandProps = ElementProps & {
  /**
   * Whether or not the dialog is open.
   */
  open?: boolean;

  /**
   * An optional open change event handler.
   * @param open Whether or not the dialog is open.
   */
  onOpenChange?: (open: boolean) => void;

  /**
   * An optional keyboard event handler.
   */
  onKeyDown?: React.KeyboardEventHandler<HTMLDivElement>;
};
