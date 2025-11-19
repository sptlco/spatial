// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A graphical element defining the outline of a shape.
 */
export const Path = createElement<{}, "path">((props, ref) => <path {...props} ref={ref} />);
