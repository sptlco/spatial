// Copyright © Spatial Corporation. All rights reserved.

import { Fragment } from "react";
import { createPortal } from "react-dom";

import { createElement } from "..";

/**
 * Portals content to another container.
 */
export const Portal = createElement<typeof Fragment, { container: Element | DocumentFragment }>((props, ref) =>
  createPortal(props.children, props.container)
);
