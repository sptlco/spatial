// Copyright © Spatial Corporation. All rights reserved.

"use client";

import * as Primitive from "@radix-ui/react-select";
import { clsx } from "clsx";
import { createContext, Fragment, ReactNode, useContext, useState } from "react";

import { Checkbox, Container, createElement, Icon, Input, ScrollArea, Span } from "..";
import { cva } from "cva";

const Context = createContext<{ multiple?: boolean }>({});

/**
 * Shared options for a select element.
 */
export type SharedSelectProps = {
  /**
   * Whether or not the menu is open.
   */
  open?: boolean;

  /**
   *
   * @param open An optional change event handler.
   * @returns
   */
  onOpenChange?: (open: boolean) => void;
};

/**
 * Configurable options for a select element that supports multiple selections.
 */
export type MultiSelectProps = {
  /**
   * Whether or not the element supports multiple selections.
   */
  multiple: true;
};

/**
 *
 */
export type SingleSelectProps = {
  /**
   * Whether or not the element supports multiple selections.
   */
  multiple?: false;
};

/**
 * Configurable options for a select element.
 */
export type SelectProps = SharedSelectProps & (SingleSelectProps | MultiSelectProps);

/**
 * Displays a list of options for the user to pick from.
 */
export const Select = {
  ...Primitive,

  /**
   * Contains all the parts of a select menu.
   */
  Root: createElement<typeof Fragment, SelectProps>(({ multiple, ...props }, _) => (
    <Context.Provider value={{ multiple }}>
      <Primitive.Root {...props} />
    </Context.Provider>
  )),

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
    ({ searchPlaceholder, position = "item-aligned", align = "center", sticky = "always", searchable = true, ...props }, ref) => {
      const content = cva({
        base: [
          "bg-background-surface shadow-base text-sm relative",
          "data-open:animate-in data-open:fade-in-0 data-open:zoom-in-95",
          "data-closed:animate-out data-closed:fade-out-0 data-closed:zoom-out-95",
          "w-fit rounded-xl duration-100 z-50 overflow-x-hidden",
          "max-h-(--radix-select-content-available-height) origin-(--radix-select-content-transform-origin)"
        ],
        variants: {
          side: {
            left: "slide-in-from-right-2",
            right: "slide-in-from-left-2",
            top: "slide-in-from-bottom-2",
            bottom: "slide-in-from-top-2"
          }
        }
      });

      const [search, setSearch] = useState("");

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
            {searchable && (
              <Container className="flex items-center gap-4 p-4">
                <Icon symbol="search" size={20} />
                <Input
                  type="text"
                  id="search"
                  name="search"
                  value={search}
                  onChange={(e) => setSearch(e.target.value)}
                  className="flex-1 min-w-0 truncate"
                  placeholder={searchPlaceholder}
                />
              </Container>
            )}
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
  Item: createElement<typeof Primitive.Item, { icon?: ReactNode; label?: ReactNode; description?: ReactNode }>((props, ref) => {
    const { multiple } = useContext(Context);

    return (
      <Container className="gap-4 w-full flex items-center">
        <Primitive.Item
          {...props}
          ref={ref}
          className={clsx(
            "p-4 gap-4 cursor-pointer transition-all flex grow items-center",
            "hover:bg-button-highlight-hover active:bg-button-highlight-active hover:text-white",
            props.className
          )}
        >
          {multiple && <Checkbox className="size-6!" />}
          {props.icon}
          <Span className="flex flex-col flex-1 min-w-0">
            {props.label && (
              <Primitive.ItemText ref={ref} className={clsx("font-extrabold whitespace-nowrap", props.className)}>
                {props.label}
              </Primitive.ItemText>
            )}
            {props.description && (
              <Span ref={ref} className={clsx("text-xs text-foreground-secondary whitespace-nowrap", props.className)}>
                {props.description}
              </Span>
            )}
          </Span>
          {!multiple && (
            <Container className="flex size-5 items-center justify-center">
              <Primitive.ItemIndicator asChild>
                <Icon symbol="check" size={20} />
              </Primitive.ItemIndicator>
            </Container>
          )}
        </Primitive.Item>
      </Container>
    );
  }),

  /**
   * A group of select items.
   */
  Group: createElement<typeof Primitive.Group, { label?: string }>((props, ref) => {
    return (
      <Primitive.Group {...props} ref={ref} className="w-full">
        <Primitive.Label ref={ref} className="flex w-full py-2 px-4 bg-background-highlight/30 font-bold text-xs text-hint uppercase">
          {props.label}
        </Primitive.Label>
        <Container className={clsx("flex flex-col w-full")}>{props.children}</Container>
      </Primitive.Group>
    );
  })
};
