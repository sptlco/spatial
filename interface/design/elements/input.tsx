// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * An element that collects data from the user.
 */
export const Input = createElement<"input">((props, ref) => <input {...props} ref={ref} />);
