// Copyright © Spatial. All rights reserved.

import { FC } from "react";
import { ElementProps } from "./ElementProps";

/**
 * A design element.
 */
export type Element<TProps extends ElementProps = ElementProps> = FC<TProps>;
