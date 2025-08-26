// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { OTPInput as Primitive, REGEXP_ONLY_DIGITS_AND_CHARS } from "input-otp";
import { Element, Field, Node, OTPProps } from "../..";
import clsx from "clsx";

/**
 * Create a new OTP element.
 * @param props Configurable options for the element.
 * @returns An OTP element.
 */
export const OTP: Element<OTPProps> = (props: OTPProps): Node => {
  return (
    <Field
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={props.className}
    >
      <Primitive
        ref={props.ref}
        autoFocus={props.autoFocus}
        maxLength={props.maxLength}
        children={props.children}
        disabled={props.disabled}
        value={props.value}
        onChange={props.onChange}
        onComplete={props.onComplete}
        pattern={REGEXP_ONLY_DIGITS_AND_CHARS}
        style={props.style}
        className={clsx("disabled:cursor-not-allowed", props.className)}
        containerClassName={clsx(
          "flex items-center space-x-1/2u",
          "has-[disabled]:opacity-50",
        )}
      />
    </Field>
  );
};
