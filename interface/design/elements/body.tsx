// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * The main content of an HTML document.
 */
export const Body = createElement<"body">((props, ref) => <body {...props} ref={ref} />);
