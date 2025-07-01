// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import * as Primitive from "@radix-ui/react-radio-group";
import { Element, ElementProps, Node, Span } from "../..";

/**
 * Create a new radio indicator element.
 * @param props Configurable options for the element.
 * @returns A radio indicator element.
 */
export const RadioIndicator: Element = (props: ElementProps): Node => {
  return (
    <Span
      className={clsx(
        "transition-all",
        "size-3/2u rounded-full",
        "flex items-center justify-center",
        "outline-border-control-default outline outline-1",
        "group-focus:!outline-2 group-active:!outline-2",
        "group-hover:outline-border-control-hover group-focus:outline-border-control-focus group-active:outline-border-control-active",
        "group-data-[state=checked]:!outline-border-control-selected-default",
        "group-data-[state=checked]:hover:!outline-border-control-selected-hover",
        "group-data-[state=checked]:focus:!outline-border-control-selected-focus",
        "group-data-[state=checked]:active:!outline-border-control-selected-active",
      )}
    >
      <Primitive.Indicator className="size-1u bg-background-accent rounded-full" />
    </Span>
  );
};
