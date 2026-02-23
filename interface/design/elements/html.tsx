// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { createElement } from "..";

/**
 * A document written in HTML.
 */
export const Html = createElement<"html">((props, ref) => (
  <html {...props} ref={ref} className={clsx("size-full overflow-hidden", "bg-background-base text-foreground-primary", props.className)} />
));
