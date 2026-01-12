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

  /**
   * Optional classes for the field's container.
   */
  containerClassName?: string;
};

/**
 * Configurable options for an OTP field.
 */
export type OTPFieldProps = OTPInputProps & {
  /**
   * The field's data type.
   */
  type: "otp";

  /**
   * An optional change event handler.
   * @param value An updated value.
   */
  onValueChange?: (value: string) => void;
};

/**
 * Configurable options for a field.
 */
export type FieldProps = SharedFieldProps & FieldTypeProps;

/**
 * Part of a form collecting user data.
 */
export const Field = createElement<"input", FieldProps>((props, ref) => {
  const render = () => {
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
            autoComplete="off"
            className={clsx(
              "disabled:opacity-50",
              "w-full px-4 py-2 bg-input placeholder-hint rounded-lg transition-all",
              "outline-2 outline-transparent focus:outline-line-input-focus",
              props.className
            )}
          />
        );
      case "otp":
        const { onValueChange, ...rest } = props;

        return (
          <OTPInput
            {...rest}
            onChange={onValueChange}
            containerClassName={clsx("has-disabled:opacity-50", "max-w-full flex flex-wrap items-center gap-4", props.className)}
            data-slot="input-otp"
          />
        );
    }
  };

  return (
    <Container className={clsx("group flex flex-col space-y-4 w-full", props.containerClassName)}>
      {props.label && (
        <Label className="px-4 font-medium transition-all" htmlFor={props.id}>
          {props.label}
        </Label>
      )}
      {render()}
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
      className={clsx("inline-flex max-w-full shrink-0 flex-wrap items-center gap-4", props.className)}
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
