// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A body of text.
 */
export const Paragraph = createElement<"p">((props, ref) => <p {...props} ref={ref} />);
