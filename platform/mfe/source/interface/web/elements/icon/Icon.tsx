// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, IconProps, Node, Span } from "..";

/**
 * Create a new icon element.
 * @param props Configurable options for the element.
 * @returns An icon element.
 */
export const Icon: Element<IconProps> = (props: IconProps): Node => {
  return <Span className={clsx("symbol", props.className)}>{props.icon}</Span>;
};
