// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * An inline container used to markup part of a text or document.
 */
export const Span = createElement<"span">((props, ref) => <span {...props} ref={ref} />);
