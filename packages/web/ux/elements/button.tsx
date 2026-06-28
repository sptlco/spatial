// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * An interactive element activated by a user with a mouse, keyboard,
 * finger, voice command, or other assistive technology.
 */
export const Button = createElement<"button">((props, ref) => {
  return <button {...props} ref={ref} />;
});
