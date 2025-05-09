// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-context-menu";
import {
  ContextMenuSubmenuTriggerProps,
  Element,
  Icon,
  Node,
  Span,
} from "../../..";
import clsx from "clsx";

/**
 * Create a new context menu submenu trigger element.
 * @param props Configurable options for the element.
 * @returns A context menu submenu trigger element.
 */
export const ContextMenuSubmenuTrigger: Element<
  ContextMenuSubmenuTriggerProps
> = ({ inset = true, ...props }: ContextMenuSubmenuTriggerProps): Node => {
  return (
    <Primitive.SubTrigger
      ref={props.ref}
      style={props.style}
      disabled={props.disabled}
      className={clsx(
        "group",
        "cursor-pointer transition-all",
        "rounded-1/2u !outline-none",
        "space-x-1/2u px-1/2u py-1/4u flex items-center",
        "data-[disabled]:pointer-events-none data-[disabled]:opacity-50",
        "data-[state=open]:bg-background-tertiary data-[highlighted]:bg-background-tertiary",
        props.className,
      )}
    >
      {(inset || props.icon) && (
        <Span className="size-3/2u flex items-center justify-center">
          {props.icon && <Icon icon={props.icon} />}
        </Span>
      )}
      <Span className="grow">{props.children}</Span>
      <Icon icon="chevron_right" />
    </Primitive.SubTrigger>
  );
};
