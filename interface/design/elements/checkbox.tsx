// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon } from "..";
import * as Primitive from "@radix-ui/react-checkbox";
import { clsx } from "clsx";

/**
 * A control that allows the user to toggle between checked and not checked.
 */
export const Checkbox = createElement<typeof Primitive.Root>((props, ref) => (
  <Primitive.Root
    {...props}
    ref={ref}
    className={clsx(
      "cursor-pointer transition-all",
      "flex shrink-0 size-6 items-center justify-center rounded-lg",
      "bg-button hover:bg-button-hover",
      "data-[state=checked]:bg-blue",
      props.className
    )}
  >
    <Primitive.Indicator className="items-center justify-center hidden data-[state=checked]:flex">
      <Icon symbol="check" size={20} className="font-normal" />
    </Primitive.Indicator>
  </Primitive.Root>
));
