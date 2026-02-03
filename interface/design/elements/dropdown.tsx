// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement } from "..";
import * as Primitive from "@radix-ui/react-dropdown-menu";
import { clsx } from "clsx";

/**
 * A menu such as a set of actions or functions, triggered by a button.
 */
export const Dropdown = {
  ...Primitive,

  /**
   * Contains all the parts of a dropdown menu.
   */
  Root: createElement<typeof Primitive.Root, Primitive.DropdownMenuProps>((props, _) => <Primitive.Root {...props} />),

  /**
   * The button that toggles the dropdown menu.
   */
  Trigger: createElement<typeof Primitive.Trigger, Primitive.DropdownMenuTriggerProps>((props, ref) => (
    <Primitive.Trigger {...props} ref={ref} className={clsx("cursor-pointer", props.className)} />
  )),

  /**
   * When used, portals the content part into the body.
   */
  Portal: createElement<typeof Primitive.Portal, Primitive.DropdownMenuPortalProps>((props, _) => <Primitive.Portal {...props} />),

  /**
   * The component that pops out when the dropdown menu is open.
   */
  Content: createElement<typeof Primitive.Content, Primitive.DropdownMenuContentProps>((props, ref) => (
    <Dropdown.Portal>
      <Primitive.Content
        {...props}
        ref={ref}
        sideOffset={20}
        collisionPadding={40}
        avoidCollisions
        className={clsx(
          "flex flex-col gap-1.5 p-3",
          "bg-background-surface text-sm shadow-lg rounded-xl w-fit md:w-screen md:max-w-3xs",
          "data-[state=open]:animate-in data-[state=closed]:animate-out data-[state=closed]:fade-out-0 data-[state=open]:fade-in-0 data-[state=closed]:zoom-out-95 data-[state=open]:zoom-in-95 data-[side=bottom]:slide-in-from-top-2 data-[side=left]:slide-in-from-right-2 data-[side=right]:slide-in-from-left-2 data-[side=top]:slide-in-from-bottom-2",
          props.className
        )}
      />
    </Dropdown.Portal>
  )),

  /**
   * An optional arrow element to render alongside the dropdown menu.
   */
  Arrow: createElement<typeof Primitive.Arrow, Primitive.DropdownMenuArrowProps>((props, ref) => <Primitive.Arrow {...props} ref={ref} />),

  /**
   * The component that contains the dropdown menu items.
   */
  Item: createElement<typeof Primitive.Item, Primitive.DropdownMenuItemProps>((props, ref) => (
    <Primitive.Item
      {...props}
      ref={ref}
      className={clsx(
        "py-2 px-4 cursor-pointer transition-all rounded-lg flex items-center hover:bg-button-hover active:bg-button-active",
        props.className
      )}
    />
  )),

  /**
   * Used to group multiple dropdown menu items.
   */
  Group: createElement<typeof Primitive.Group, Primitive.DropdownMenuGroupProps>((props, ref) => <Primitive.Group {...props} ref={ref} />),

  /**
   * Used to render a label.
   */
  Label: createElement<typeof Primitive.Label, Primitive.DropdownMenuLabelProps>((props, ref) => <Primitive.Label {...props} ref={ref} />),

  /**
   * An item that can be controlled and rendered like a checkbox.
   */
  CheckboxItem: createElement<typeof Primitive.CheckboxItem>((props, ref) => (
    <Primitive.CheckboxItem
      {...props}
      ref={ref}
      className={clsx(
        "py-2 px-4 cursor-pointer transition-all rounded-lg flex items-center hover:bg-button-hover active:bg-button-active",
        props.className
      )}
    />
  )),

  /**
   * Used to group multiple dropdown radio items.
   */
  RadioGroup: createElement<typeof Primitive.RadioGroup, Primitive.DropdownMenuRadioGroupProps>((props, ref) => (
    <Primitive.RadioGroup {...props} ref={ref} />
  )),

  /**
   * An item that can be controlled and rendered like a radio.
   */
  RadioItem: createElement<typeof Primitive.RadioItem, Primitive.DropdownMenuRadioItemProps>((props, ref) => (
    <Primitive.RadioItem
      {...props}
      ref={ref}
      className={clsx(
        "py-2 px-4 cursor-pointer transition-all rounded-lg flex items-center hover:bg-button-hover active:bg-button-active",
        props.className
      )}
    />
  )),

  /**
   * Renders when the parent dropdown checkbox or radio item is checked.
   */
  ItemIndicator: createElement<typeof Primitive.ItemIndicator, Primitive.DropdownMenuItemIndicatorProps>((props, ref) => (
    <Primitive.ItemIndicator {...props} ref={ref} />
  )),

  /**
   * Used to visually separate dropdown menu items.
   */
  Separator: createElement<typeof Primitive.Separator, Primitive.DropdownMenuSeparatorProps>((props, ref) => (
    <Primitive.Separator {...props} ref={ref} />
  )),

  /**
   * Contains all the parts of a submenu.
   */
  Sub: createElement<typeof Primitive.Sub, Primitive.DropdownMenuSubProps>((props, ref) => <Primitive.Sub {...props} />),

  /**
   * An item that opens a submenu.
   */
  SubTrigger: createElement<typeof Primitive.SubTrigger, Primitive.DropdownMenuSubTriggerProps>((props, ref) => (
    <Primitive.SubTrigger {...props} ref={ref} />
  )),

  /**
   * The component that pops out when a submenu is open.
   */
  SubContent: createElement<typeof Primitive.SubContent, Primitive.DropdownMenuSubContentProps>((props, ref) => (
    <Primitive.SubContent {...props} ref={ref} />
  ))
};
