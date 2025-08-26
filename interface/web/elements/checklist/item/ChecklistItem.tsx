// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { ChecklistItemProps, Element, Icon, Node, Span } from "@spatial/elements";

/**
 * Create a new checklist item element.
 * @param props Configurable options for the element.
 * @returns A checklist item element.
 */
export const ChecklistItem: Element<ChecklistItemProps> = (
  props: ChecklistItemProps,
): Node => {
  return (
    <Span className="space-x-1u flex items-start">
      <Span
        className={clsx(
          "shrink-0",
          "size-3/2u flex items-center justify-center rounded-full",
          "text-base-white-9 bg-base-red-5",
          { "!bg-base-green-5": props.checked },
        )}
      >
        <Icon className="!text-m" icon={props.checked ? "check" : "close"} />
      </Span>
      <Span
        className={clsx("overflow-hidden", "text-ellipsis whitespace-nowrap")}
      >
        {props.children}
      </Span>
    </Span>
  );
};
