// Copyright © Spatial. All rights reserved.

import { FormEventHandler } from "react";
import { ElementProps } from "..";

/**
 * Configurable options for a form element.
 */
export type FormProps = ElementProps & {
  /**
   * An optional submit event handler.
   */
  onSubmit?: FormEventHandler<HTMLFormElement>;
};
