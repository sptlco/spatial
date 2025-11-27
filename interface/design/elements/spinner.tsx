// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { createElement, Span } from "..";

/**
 * A graphical element that signals a loading status.
 */
export const Spinner = createElement<"span">((props, ref) => (
  <Span
    {...props}
    ref={ref}
    className={clsx("animate-spin rounded-full border-2 border-current border-t-transparent inline-flex", props.className)}
  />
));
