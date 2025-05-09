// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Div, Element, ElementProps, Node } from "..";

/**
 * Create a new carousel slide element.
 * @param props Configurable options for the element.
 * @returns A carousel slide element.
 */
export const CarouselSlide: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      style={props.style}
      className={clsx(
        "embla__slide",
        "size-full shrink-0 grow-0",
        props.className,
      )}
      children={props.children}
    />
  );
};
