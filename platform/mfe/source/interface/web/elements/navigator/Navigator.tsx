// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Element, NavigatorProps, NavigatorViewport, Node } from "..";
import clsx from "clsx";

/**
 * Create a new navigator element.
 * @param props Configurable options for the element.
 * @returns A navigator element.
 */
export const Navigator: Element<NavigatorProps> = ({
  orientation = "horizontal",
  ...props
}: NavigatorProps): Node => {
  return (
    <Primitive.Root
      ref={props.ref}
      style={props.style}
      defaultValue={props.defaultValue}
      value={props.value}
      onValueChange={props.onChange}
      orientation={orientation}
      className={clsx(
        "relative",
        "max-w-fit",
        "flex data-[orientation=horizontal]:items-center",
        "data-[orientation=horizontal]:justify-center",
        "data-[orientation=vertical]:flex-col",
        props.className,
      )}
    >
      {props.children}
      {orientation == "horizontal" && <NavigatorViewport />}
    </Primitive.Root>
  );
};
