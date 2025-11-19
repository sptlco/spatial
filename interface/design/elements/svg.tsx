// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A two-dimensional vector graphic element allowing for scalable, high-quality
 * images that maintain clarity at any size without pixelation.
 */
export const Svg = createElement<{}, "svg">((props, ref) => <svg {...props} ref={ref} xmlns="http://www.w3.org/2000/svg" />);
