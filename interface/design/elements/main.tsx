// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * The dominant content of a document.
 */
export const Main = createElement<"main">((props, ref) => <main {...props} ref={ref} />);
