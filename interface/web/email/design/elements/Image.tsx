// Copyright © Spatial Corporation. All rights reserved.

import { Img as Primitive } from "@react-email/components";
import { Element, ElementProps, Node } from ".";

/**
 * Create a new image element.
 * @param props Configurable options for the element.
 * @returns An image element.
 */
export const Image: Element<ImageProps> = (props: ImageProps): Node => {
  return (
    <Primitive
      src={props.src}
      style={props.style}
      className={props.className}
      children={props.children}
    />
  );
};

type ImageProps = ElementProps & {
  src: string;
};
