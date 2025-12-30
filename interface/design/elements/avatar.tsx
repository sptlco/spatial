// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Image, ImageProps } from "..";
import { clsx } from "clsx";

/**
 * An image element representing the user.
 */
export const Avatar = createElement<"img", ImageProps>((props, ref) => (
  <Image {...props} ref={ref} className={clsx("rounded-full overflow-hidden object-center object-cover aspect-square", props.className)} />
));
