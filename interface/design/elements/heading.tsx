// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A heading at level 1.
 */
export const H1 = createElement<"h1">((props, ref) => <h1 {...props} ref={ref} />);

/**
 * A heading at level 2.
 */
export const H2 = createElement<"h2">((props, ref) => <h2 {...props} ref={ref} />);

/**
 * A heading at level 3.
 */
export const H3 = createElement<"h3">((props, ref) => <h3 {...props} ref={ref} />);

/**
 * A heading at level 4.
 */
export const H4 = createElement<"h4">((props, ref) => <h4 {...props} ref={ref} />);

/**
 * A heading at level 5.
 */
export const H5 = createElement<"h5">((props, ref) => <h5 {...props} ref={ref} />);

/**
 * A heading at level 6.
 */
export const H6 = createElement<"h6">((props, ref) => <h6 {...props} ref={ref} />);
