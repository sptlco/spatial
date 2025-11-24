// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A document written in HTML.
 */
export const Html = createElement<"html">((props, ref) => <html {...props} ref={ref} />);
