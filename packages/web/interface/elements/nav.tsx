// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A site navigation element.
 */
export const Nav = createElement<"nav">((props, ref) => <nav {...props} ref={ref} />);
