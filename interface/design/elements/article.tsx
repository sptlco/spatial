// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A body of text.
 */
export const Article = createElement<"article">((props, ref) => <article {...props} ref={ref} />);
