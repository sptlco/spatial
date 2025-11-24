// Copyright © Spatial Corporation. All rights reserved.

import { clsx } from "clsx";
import { Fragment, ReactNode } from "react";
import { Container, createElement, Input, Label, Paragraph } from "..";

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
export type FieldTypeProps = TextFieldProps;

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
 * Configurable options for a field.
 */
export type FieldProps = SharedFieldProps & FieldTypeProps;

/**
 * Part of a form collecting user data.
 */
export const Field = createElement<typeof Fragment, FieldProps>((props, _) => {
  const Control = (): ReactNode => {
    switch (props.type) {
      case "text":
      case "email":
      case "password":
        return (
          <Input
            type={props.type}
            placeholder={props.placeholder}
            className={clsx(
              "w-full px-4 py-2 bg-input placeholder-hint rounded-lg transition-all",
              "outline-2 outline-transparent outline-offset-2 focus:outline-line-input-focus"
            )}
          />
        );
    }
  };

  return (
    <Container className="group flex flex-col space-y-4 w-full">
      {props.label && (
        <Label className="px-4 text-sm font-bold uppercase transition-all group-focus-within:text-line-input-focus">{props.label}</Label>
      )}
      <Control />
      {props.description && <Paragraph className="px-4 text-sm text-foreground-secondary">{props.description}</Paragraph>}
    </Container>
  );
});
