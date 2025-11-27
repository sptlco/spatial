// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { OTPInput, OTPInputContext, OTPInputProps } from "input-otp";
import { ReactNode, useContext } from "react";
import { Container, createElement, Input, Label, Paragraph, Span } from "..";

/**
 * Common configurable options for a field.
 */
export interface SharedFieldProps {
  /**
   * An optional label for the field.
   */
  label?: ReactNode;

  /**
   * An optional description for the field.
   */
  description?: ReactNode;
}

/**
 * Configurable options for a specific field type.
 */
export type FieldTypeProps = TextFieldProps | OTPFieldProps;

/**
 * Configurable options for a text field.
 */
export type TextFieldProps = {
  /**
   * The field's data type.
   */
  type: "text" | "email" | "password";

  /**
   * An optional placeholder for the field.
   */
  placeholder?: string;
};

/**
 * Configurable options for an OTP field.
 */
export type OTPFieldProps = OTPInputProps & {
  /**
   * The field's data type.
   */
  type: "otp";
};

/**
 * Configurable options for a field.
 */
export type FieldProps = SharedFieldProps & FieldTypeProps;

/**
 * Part of a form collecting user data.
 */
export const Field = createElement<"input", FieldProps>((props, ref) => {
  const Control = (): ReactNode => {
    switch (props.type) {
      case "text":
      case "email":
      case "password":
        return (
          <Input
            {...props}
            ref={ref}
            type={props.type}
            placeholder={props.placeholder}
            className={clsx(
              "disabled:opacity-50",
              "w-full px-4 py-2 bg-input placeholder-hint rounded-lg transition-all",
              "outline-2 outline-transparent focus:outline-line-input-focus"
            )}
          />
        );
      case "otp":
        return (
          <OTPInput
            {...props}
            containerClassName={clsx("disabled:opacity-50", "w-full flex flex-wrap items-center justify-center gap-4")}
            data-slot="input-otp"
          />
        );
    }
  };

  return (
    <Container className="group flex flex-col space-y-4 w-full">
      {props.label && <Label className="px-4 font-medium transition-all">{props.label}</Label>}
      <Control />
      {props.description && <Paragraph className="px-4 text-sm transition-all text-foreground-secondary">{props.description}</Paragraph>}
    </Container>
  );
});

/**
 * A one-time password field.
 */
export const OTP = {
  /**
   * A group of one-time password slots.
   */
  Group: createElement<typeof Container>((props, ref) => (
    <Container
      {...props}
      ref={ref}
      data-slot="input-otp-group"
      className={clsx("flex shrink-0 flex-wrap justify-center items-center gap-4", props.className)}
    />
  )),

  /**
   * A one-time password slot.
   */
  Slot: createElement<typeof Container, { index: number }>((props, ref) => {
    const context = useContext(OTPInputContext);
    const { char, hasFakeCaret, isActive } = context?.slots[props.index] ?? {};

    const Caret = () => (
      <Container className="pointer-events-none absolute inset-0 flex items-center justify-center">
        <Container className="animate-caret bg-current h-4 w-px duration-1000" />
      </Container>
    );

    return (
      <Container
        {...props}
        ref={ref}
        data-slot="input-otp-slot"
        data-active={isActive}
        className={clsx(
          "relative",
          "flex shrink-0 items-center justify-center size-10 transition-all",
          "bg-input rounded-lg outline-2 outline-transparent data-[active=true]:outline-line-input-focus",
          props.className
        )}
      >
        {char}
        {hasFakeCaret && <Caret />}
      </Container>
    );
  }),

  /**
   * An element that separates slots of a one-time password field.
   */
  Separator: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} data-slot="input-otp-separator" role="separator">
      <Span className="flex h-px w-2 rounded-full bg-line-base" />
    </Container>
  ))
};
