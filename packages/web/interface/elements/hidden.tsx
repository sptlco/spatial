// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import { VisuallyHidden, type VisuallyHiddenProps } from "@radix-ui/react-visually-hidden";

/**
 * A visually hidden element.
 */
export const Hidden = createElement<typeof VisuallyHidden, VisuallyHiddenProps>((props, _) => <VisuallyHidden {...props} />);
