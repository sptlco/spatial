// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { createContext, ReactNode, useContext, useEffect, useState } from "react";

import { Container, createElement, Icon, Input, LI, Popover, Span, UL } from "..";

/**
 * Configurable options for a combobox.
 */
export type ComboboxProps = SharedComboboxProps & (SingleComboboxProps | MultiComboboxProps);

/**
 * Configurable options shared by all comboboxes.
 */
export type SharedComboboxProps = {
  /**
   * An optional selection event handler.
   * @param value The value that was selected.
   */
  onSelect?: (value: string) => void;
};

/**
 * Configurable options for a combobox under single-selection mode.
 */
export type SingleComboboxProps = {
  /**
   * Whether or not multiple selections are supported.
   */
  multiple?: false;

  /**
   * The box's current selection.
   */
  selection?: string;
};

/**
 * Configurable options for a combobox under multi-selection mode.
 */
export type MultiComboboxProps = {
  /**
   * Whether or not multiple selections are supported.
   */
  multiple: true;

  /**
   * The box's current selection.
   */
  selection?: string[];
};

/**
 * Internal context responsible for the behavior of a combobox.
 */
type ComboboxContext = {
  multiple?: boolean;
  search: string[];
  setSearch: (search: string[]) => void;
  selection: string[];
  onSelect?: (value: string) => void;
};

const Context = createContext<ComboboxContext>({ search: [], setSearch: () => {}, selection: [] });

/**
 * A searchable list of items.
 */
export const Combobox = {
  /**
   * Contains all the parts of a combobox.
   */
  Root: createElement<typeof Popover.Root, ComboboxProps>((props, _) => {
    const [search, setSearch] = useState<string[]>([]);
    const [selection, setSelection] = useState<string[]>([]);

    const context: ComboboxContext = {
      ...props,
      search,
      setSearch,
      selection
    };

    useEffect(() => {
      if (props.selection) {
        setSelection(props.multiple ? props.selection : [props.selection]);
      }
    }, [props.selection]);

    return (
      <Context.Provider value={context}>
        <Popover.Root {...props} />
      </Context.Provider>
    );
  }),

  /**
   * The button that opens the combobox.
   */
  Trigger: createElement<typeof Popover.Trigger>((props, ref) => <Popover.Trigger {...props} ref={ref} />),

  /**
   * The component displayed when the combobox opens.
   */
  Content: createElement<typeof Popover.Content, { searchPlaceholder?: string }>(({ searchPlaceholder = "Search", ...props }, ref) => {
    const { setSearch } = useContext(Context);

    const [value, setValue] = useState("");

    return (
      <Popover.Content {...props} ref={ref}>
        <Container className="flex items-center gap-4 p-4">
          <Icon symbol="search" size={20} />
          <Input
            type="text"
            id="search"
            name="search"
            className="flex-1 min-w-0 truncate"
            placeholder={searchPlaceholder}
            autoComplete="off"
            value={value}
            onChange={(e) => {
              setValue(e.target.value);
              setSearch(e.target.value.trim().split(/\s+/).filter(Boolean));
            }}
          />
        </Container>
        <Popover.Viewport className="max-h-[calc(var(--radix-popover-content-available-height)-52px)]">{props.children}</Popover.Viewport>
      </Popover.Content>
    );
  }),

  /**
   * A list of combobox items.
   */
  List: createElement<typeof UL, { label?: string; containerClassName?: string }>((props, ref) => (
    <Container className={clsx("flex flex-col w-full", props.containerClassName)}>
      {props.label && <Span className="flex w-full py-2 px-4 bg-background-highlight/30 font-bold text-xs text-hint uppercase">{props.label}</Span>}
      <UL {...props} ref={ref} className={clsx("flex flex-col w-full", props.className)}>
        {props.children}
      </UL>
    </Container>
  )),

  /**
   * An item listed in the combobox.
   */
  Item: createElement<typeof LI, { value: string; label: string; icon?: ReactNode; description?: string; indicator?: ReactNode }>((props, ref) => {
    const { multiple, search, selection, onSelect } = useContext(Context);

    const selected = selection?.includes(props.value);
    const matches =
      search.length <= 0 ||
      search.every(
        (keyword) => props.label?.toLowerCase().includes(keyword.toLowerCase()) || props.description?.toLowerCase().includes(keyword.toLowerCase())
      );

    if (!matches) {
      return null;
    }

    const select = () => {
      if (onSelect && (multiple || !selected)) {
        onSelect(props.value);
      }
    };

    return (
      <LI
        {...props}
        ref={ref}
        onClick={select}
        className={clsx(
          "p-4 gap-4 cursor-pointer transition-all flex grow items-center",
          "hover:bg-button-highlight-hover active:bg-button-highlight-active hover:text-white",
          props.className
        )}
      >
        {props.icon}
        <Span className="flex flex-col truncate flex-1">
          {props.label && <Span className="truncate">{highlight(props.label, search)}</Span>}
          {props.description && <Span className="text-sm text-foreground-secondary truncate">{highlight(props.description, search)}</Span>}
        </Span>
        <Container className="flex size-5 items-center justify-center">
          {selected && (props.indicator || (multiple ? <Span className="flex size-2 rounded-full bg-blue" /> : <Icon symbol="check" size={20} />))}
        </Container>
      </LI>
    );
  })
};

const highlight = (text: string, keywords: string[]) => {
  if (!keywords.length) {
    return text;
  }

  const escaped = keywords.map((k) => k.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"));
  const regex = new RegExp(`(${escaped.join("|")})`, "gi");

  return text.split(regex).map((part, i) => {
    return keywords.some((keyword) => part.toLowerCase() === keyword.toLowerCase()) ? (
      <mark key={i} className="rounded px-1 bg-yellow text-foreground-inverse font-bold">
        {part}
      </mark>
    ) : (
      <span key={i}>{part}</span>
    );
  });
};
