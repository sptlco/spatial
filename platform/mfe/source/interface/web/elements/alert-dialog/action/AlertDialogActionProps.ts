// Copyright © Spatial. All rights reserved.

import { ElementProps } from "../..";

/**
 * Configurable options for an alert dialog action element.
 */
export type AlertDialogActionProps = ElementProps & {
  /**
   * Whether or not to render the element as its child.
   */
  asChild?: boolean;
};
