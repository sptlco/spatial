// Copyright © Spatial Corporation. All rights reserved.

import { FC } from "react";
import { ElementProps } from ".";

/**
 * A mail element.
 */
export type Element<T extends ElementProps = ElementProps> = FC<T>;
