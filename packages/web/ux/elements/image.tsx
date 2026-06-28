// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";

/**
 * A graphical element that displays visual content, such as a photo, icon, or illustration.
 */
export const Image = createElement<"img">((props, ref) => {
  return <img {...props} ref={ref} src={props.src} />;
});
