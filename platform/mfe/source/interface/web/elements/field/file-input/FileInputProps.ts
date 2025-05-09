// Copyright © Spatial. All rights reserved.

import { FieldProps } from "@spatial/elements";

/**
 * Configurable options for a file input element.
 */
export type FileInputProps = FieldProps & {
  /**
   * The field's value.
   */
  value?: File[];

  /**
   * An optional change event handler.
   * @param files The selected files.
   */
  onChange?: (files: FileList) => void;

  /**
   * An optional string indicating what files to accept.
   */
  accept?: string;
};
