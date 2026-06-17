// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A group of elements collecting user data.
 */
export const Form = createElement<"form">((props, ref) => <form {...props} ref={ref} />);
