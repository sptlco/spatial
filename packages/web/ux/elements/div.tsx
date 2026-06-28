// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A container element.
 */
export const Div = createElement<"div">((props, ref) => <div {...props} ref={ref} />);
