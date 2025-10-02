// Copyright © Spatial Corporation. All rights reserved.

import { FC, ReactNode, SVGProps } from "react";

/**
 * Create a new symbol component.
 * @param props Configurable options for the component.
 * @returns A symbol component.
 */
export const Symbol: FC<SVGProps<SVGSVGElement>> = (props): ReactNode => {
  return (
    <svg viewBox="0 0 600 400" {...props}>
      <path d="M590.169 137.158H455.086L291.046 292.397C284.301 298.786 270.447 304.007 260.26 304.007H154.49L64.9893 388.695C58.4377 394.917 61.1583 400 71.0689 400H312.422C322.5 400 336.213 394.834 342.903 388.501L596.361 148.629C603.051 142.297 600.247 137.13 590.169 137.13V137.158Z" />
      <path d="M280.943 251.371L534.401 11.4992C541.091 5.1663 538.287 0 528.21 0H287.578C277.501 0 263.787 5.1663 257.097 11.4992L3.63962 251.371C-3.05076 257.705 -0.246909 262.871 9.8303 262.871H250.462C260.538 262.871 274.253 257.705 280.943 251.371Z" />
    </svg>
  );
};
