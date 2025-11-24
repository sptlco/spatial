// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A header for an HTML document.
 */
export const Head = createElement<"head">((props, ref) => <head {...props} ref={ref} />);
