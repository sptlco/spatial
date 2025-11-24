// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A container element.
 */
export const Container = createElement<"div">((props, ref) => <div {...props} ref={ref} />);
