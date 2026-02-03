// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { OTPInput, OTPInputContext, OTPInputProps } from "input-otp";
import { ReactNode, useContext, useEffect, useRef, useState } from "react";
import { HexColorPicker, HexColorInput } from "react-colorful";
import { Button, Container, createElement, Dropdown, Icon, Input, Label, Paragraph, Span } from "..";

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

  /**
   * Whether or not the field's label is inset.
   */
  inset?: boolean;

  /**
   * Optional classes for the field's container.
   */
  containerClassName?: string;
}

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

  /**
   * An optional change event handler.
   * @param value An updated value.
   */
  onValueChange?: (value: string) => void;
};

type Metadata = Record<string, string>;
type Row = { key: string; value: string };

/**
 * Configurable options for a meta-field.
 */
export type MetaFieldProps = {
  /**
   * The field's data type.
   */
  type: "meta";

  /**
   * The field's value.
   */
  metadata?: Metadata;

  /**
   * An optional change event handler.
   * @param value The field's new value.
   */
  onValueChange?: (value: Metadata) => void;
};

/**
 * Configurable options for a color field.
 */
export type ColorFieldProps = {
  /**
   * The field's data type.
   */
  type: "color";

  /**
   * The field's value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The field's new value.
   */
  onValueChange?: (value: string) => void;
};

/**
 * Configurable options for a specific field type.
 */
export type FieldTypeProps = TextFieldProps | OTPFieldProps | MetaFieldProps | ColorFieldProps;

/**
 * Configurable options for a field.
 */
export type FieldProps = SharedFieldProps & FieldTypeProps;

/**
 * Part of a form collecting user data.
 */
export const Field = createElement<"input", FieldProps>(({ inset = true, ...props }, ref) => {
  const render = () => {
    const defaultClasses = clsx(
      "disabled:opacity-50",
      "w-full px-4 py-2 bg-input placeholder-hint rounded-lg transition-all",
      "outline-2 outline-offset-3 outline-transparent focus:outline-line-input-focus",
      props.className
    );

    switch (props.type) {
      case "text":
      case "email":
      case "password":
        return <Input {...props} ref={ref} type={props.type} placeholder={props.placeholder} autoComplete="off" className={defaultClasses} />;
      case "otp": {
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
      case "color": {
        const { onValueChange, ...rest } = props;
        const [open, setOpen] = useState(false);

        return (
          <Container className="relative w-full flex items-center gap-4">
            <Dropdown.Root open={open} onOpenChange={setOpen}>
              <Dropdown.Trigger className="flex shrink-0 size-10 items-center justify-center cursor-pointer">
                <Span className="size-full rounded-lg" style={{ backgroundColor: rest.value }} />
              </Dropdown.Trigger>
              <Dropdown.Content className="bg-transparent! shadow-none! z-80! w-fit!">
                <HexColorPicker color={rest.value} onChange={onValueChange} />
              </Dropdown.Content>
            </Dropdown.Root>
            <HexColorInput {...rest} type="text" className={defaultClasses} color={rest.value} prefixed onChange={onValueChange} />
          </Container>
        );
      }
      case "meta":
        const [rows, setRows] = useState<Row[]>([]);

        const history = useRef<Metadata | undefined>(props.metadata);

        useEffect(() => {
          if (!props.metadata || JSON.stringify(props.metadata) === JSON.stringify(history.current)) {
            return;
          }

          history.current = props.metadata;

          const next = Object.entries(props.metadata).map(([key, value]) => ({
            key,
            value: String(value)
          }));

          if (next.length === 0) {
            next.push({ key: "", value: "" });
          }

          setRows(next);
        }, [props.metadata]);

        useEffect(() => {
          if (!props.onValueChange) return;

          const metadata = rows.reduce<Metadata>((acc, row) => {
            const key = row.key.trim();

            if (key !== "") {
              acc[key] = row.value;
            }

            return acc;
          }, {});

          if (JSON.stringify(metadata) !== JSON.stringify(history.current)) {
            history.current = metadata;
            props.onValueChange(metadata);
          }
        }, [rows, props.onValueChange]);

        const update = (index: number, patch: Partial<Row>) => {
          setRows((rows) => {
            const next = rows.map((row, i) => (i === index ? { ...row, ...patch } : row));

            if (index === rows.length - 1 && patch.key?.trim() && patch.value?.trim()) {
              return [...next, { key: "", value: "" }];
            }

            return next;
          });
        };

        const remove = (index: number) => {
          setRows((rows) => {
            const next = rows.filter((_, i) => i !== index);

            if (next.length === 0) {
              return [{ key: "", value: "" }];
            }

            return next;
          });
        };

        const add = () => {
          setRows((rows) => [...rows, { key: "", value: "" }]);
        };

        return (
          <Container className="flex flex-col gap-4">
            {rows.map((row, i) => (
              <Container key={i} className="flex items-center gap-4">
                <Field
                  type="text"
                  placeholder="Key"
                  value={row.key}
                  disabled={props.disabled}
                  inset={false}
                  onChange={(e) => update(i, { key: e.target.value })}
                />
                <Span>:</Span>
                <Field
                  type="text"
                  placeholder="Value"
                  value={row.value}
                  disabled={props.disabled}
                  inset={false}
                  onChange={(e) => update(i, { value: e.target.value })}
                />
                <Button type="button" intent="ghost" className="bg-transparent! px-0!" disabled={props.disabled} onClick={() => remove(i)}>
                  <Icon symbol="close" />
                </Button>
              </Container>
            ))}

            <Button type="button" intent="ghost" className="bg-transparent! px-0!" disabled={props.disabled} onClick={add}>
              <Icon symbol="add" />
              <Span>Add property</Span>
            </Button>
          </Container>
        );
    }
  };

  return (
    <Container className={clsx("group flex flex-col space-y-4 w-full", props.containerClassName)}>
      {props.label && (
        <Label className={clsx("font-medium transition-all", { "px-4": inset })} htmlFor={props.id}>
          {props.label}
        </Label>
      )}
      {render()}
      {props.description && (
        <Paragraph className={clsx("text-sm transition-all text-foreground-secondary", { "px-4": inset })}>{props.description}</Paragraph>
      )}
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
