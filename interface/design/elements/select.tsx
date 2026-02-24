// Copyright © Spatial Corporation. All rights reserved.

"use client";

import * as Primitive from "@radix-ui/react-select";
import { clsx } from "clsx";
import { cva } from "cva";
import { Fragment, ReactNode } from "react";

import { Container, createElement, Icon, ScrollArea, Span } from "..";

/**
 * Configurable options for a select element.
 */
export type SelectProps = {
  /**
   * The selected value.
   */
  value?: string;

  /**
   * An optional change event handler.
   * @param value The selected value.
   */
  onValueChange?: (value: string) => void;
};

/**
 * Displays a list of options for the user to pick from.
 */
export const Select = {
  ...Primitive,

  /**
   * Contains all the parts of a select menu.
   */
  Root: createElement<typeof Fragment, SelectProps>((props, _) => <Primitive.Root {...props} />),

  /**
   * Opens the select menu.
   */
  Trigger: createElement<typeof Primitive.Trigger>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} className={clsx("data-disabled:opacity-50", props.className)} />
  )),

  /**
   * The component that pops out when the select menu is open.
   */
  Content: createElement<typeof Primitive.Content, { searchable?: boolean; searchPlaceholder?: string }>(
    ({ position = "popper", align = "center", sticky = "always", ...props }, ref) => {
      const content = cva({
        base: [
          "bg-background-surface shadow-base relative",
          "data-[state=open]:animate-in data-[state=open]:fade-in-0 data-[state=open]:zoom-in-95",
          "data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=closed]:zoom-out-95",
          "w-fit rounded-xl duration-100 z-50 overflow-x-hidden",
          "max-h-(--radix-select-content-available-height) origin-(--radix-select-content-transform-origin)"
        ],
        variants: {
          side: {
            left: "data-[state=open]:slide-in-from-right-2 data-[state=closed]:slide-out-to-right-2",
            right: "data-[state=open]:slide-in-from-left-2 data-[state=closed]:slide-out-to-left-2",
            top: "data-[state=open]:slide-in-from-bottom-2 data-[state=closed]:slide-out-to-bottom-2",
            bottom: "data-[state=open]:slide-in-from-top-2 data-[state=closed]:slide-out-to-top-2"
          }
        }
      });

      return (
        <Primitive.Portal>
          <Primitive.Content
            {...props}
            ref={ref}
            position={position}
            align={align}
            sideOffset={20}
            collisionPadding={40}
            sticky={sticky}
            hideWhenDetached
            avoidCollisions
            className={clsx(content({ side: props.side }), props.className)}
          >
            <ScrollArea.Root className="grow" type="auto" fade>
              <Primitive.Viewport
                style={{ overflowY: undefined }}
                className="max-h-[calc(var(--radix-select-content-available-height)-52px)]"
                asChild
              >
                <ScrollArea.Viewport children={props.children} />
              </Primitive.Viewport>
              <ScrollArea.Scrollbar />
            </ScrollArea.Root>
          </Primitive.Content>
        </Primitive.Portal>
      );
    }
  ),

  /**
   * A selectable option.
   */
  Item: createElement<typeof Primitive.Item, { icon?: ReactNode; indicator?: ReactNode; label?: string; description?: string }>((props, ref) => {
    return (
      <Primitive.Item
        {...props}
        ref={ref}
        className={clsx(
          "p-4 gap-4 cursor-pointer transition-all flex grow items-center",
          "hover:bg-button-highlight-hover active:bg-button-highlight-active hover:text-white",
          props.className
        )}
      >
        {props.icon}
        <Span className="flex flex-col truncate flex-1">
          {props.label && <Primitive.ItemText className="font-extrabold truncate">{props.label}</Primitive.ItemText>}
          {props.description && <Span className="text-sm text-foreground-secondary truncate">{props.description}</Span>}
        </Span>
        <Container className="flex size-5 items-center justify-center">
          <Primitive.ItemIndicator asChild>{props.indicator || <Icon symbol="check" size={20} />}</Primitive.ItemIndicator>
        </Container>
      </Primitive.Item>
    );
  }),

  /**
   * A group of select items.
   */
  Group: createElement<typeof Primitive.Group, { label?: string }>((props, ref) => {
    return (
      <Primitive.Group {...props} ref={ref} className="w-full">
        {props.label && (
          <Primitive.Label className="flex w-full py-2 px-4 bg-background-highlight/30 font-bold text-xs text-hint uppercase">
            {props.label}
          </Primitive.Label>
        )}
        <Container className={clsx("flex flex-col w-full")}>{props.children}</Container>
      </Primitive.Group>
    );
  })
};
