// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { Div, Element, ElementProps, Icon, Node } from "../..";

/**
 * Create a new OTP separator element.
 * @param props Configurable options for the element.
 * @returns An OTP separator element.
 */
export const OTPSeparator: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      role="separator"
      style={props.style}
      className={clsx(
        "size-1/4u bg-border-bounds inline-flex shrink-0 rounded-full",
      )}
    />
  );
};
