// Copyright © Spatial Corporation. All rights reserved.

import { createElement, resolve } from "..";

/**
 * Configurable options for an image.
 */
export type ImageProps = {
  /**
   * The URL pointing to the image file.
   */
  src: string;
};

/**
 * A graphical element that displays visual content, such as a photo, icon, or illustration.
 */
export const Image = createElement<"img", ImageProps>((props, ref) => {
  return <img {...props} ref={ref} src={resolve(props.src)} />;
});
