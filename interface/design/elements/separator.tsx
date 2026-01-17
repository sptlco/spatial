// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import * as Primitive from "@radix-ui/react-separator";

/**
 * Visually or semantically separates content.
 */
export const Separator = createElement<typeof Primitive.Root>((props, ref) => <Primitive.Root {...props} ref={ref} />);
