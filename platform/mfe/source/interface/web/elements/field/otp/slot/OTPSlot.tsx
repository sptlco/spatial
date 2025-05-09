// Copyright © Spatial. All rights reserved.

"use client";

import { useContext } from "react";
import { Div, Element, Node, OTPSlotProps } from "../../..";
import { OTPInputContext } from "input-otp";
import clsx from "clsx";

/**
 * Create a new OTP slot element.
 * @param props Configurable options for the element.
 * @returns An OTP slot element.
 */
export const OTPSlot: Element<OTPSlotProps> = (props: OTPSlotProps): Node => {
  const context = useContext(OTPInputContext);
  const { char, hasFakeCaret, isActive } = context.slots[props.index];

  return (
    <Div
      ref={props.ref}
      className={clsx(
        "transition-all",
        "size-5/2u relative",
        "flex shrink-0 items-center justify-center",
        "rounded-1/2u uppercase",
        "bg-background-control-default border-border-control-default border-1 border",
        "hover:border-border-control-hover",
        {
          "outline-border-control-focus z-10 outline outline-2": isActive,
        },
        props.className,
      )}
    >
      {char}
      {hasFakeCaret && (
        <div className="pointer-events-none absolute inset-0 flex items-center justify-center">
          <div className="h-3/2u animate-caret-blink w-[1px] bg-[currentColor] duration-1000" />
        </div>
      )}
    </Div>
  );
};
