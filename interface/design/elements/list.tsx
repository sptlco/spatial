// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * An unordered list of elements.
 */
export const UL = createElement<"ul">((props, ref) => <ul {...props} ref={ref} />);

/**
 * An ordered list of elements.
 */
export const OL = createElement<"ol">((props, ref) => <ol {...props} ref={ref} />);

/**
 * A listed element.
 */
export const LI = createElement<"li">((props, ref) => <li {...props} ref={ref} />);
