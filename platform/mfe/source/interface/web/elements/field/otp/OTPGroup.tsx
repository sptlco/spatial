// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import {Div, Element, ElementProps, Node} from "../..";

/**
 * Create a new OTP group element.
 * @param props Configurable options for the element.
 * @returns An OTP group element.
 */
export const OTPGroup: Element = (props: ElementProps): Node => {
  return (
    <Div
      ref={props.ref}
      children={props.children}
      style={props.style}
      className={clsx(
        "flex w-full items-center justify-between",
        props.className,
      )}
    />
  );
};
