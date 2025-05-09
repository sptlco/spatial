// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-navigation-menu";
import { Div, Element, ElementProps, Node } from "..";
import clsx from "clsx";

/**
 * Create a new navigator viewport element.
 * @param props Configurable options for the element.
 * @returns A navigator viewport element.
 */
export const NavigatorViewport: Element = (props: ElementProps): Node => {
  return (
    <Div className="absolute top-full flex w-screen justify-center">
      <Primitive.Viewport
        ref={props.ref}
        style={props.style}
        children={props.children}
        className={clsx(
          "z-10",
          "relative",
          "transition-all",
          "origin-top-center mt-1/2u",
          "overflow-hidden",
          "h-[var(--radix-navigation-menu-viewport-height)]",
          "flex w-full items-center justify-center",
          "bg-background-primary",
          "data-[state=open]:animate-in data-[state=closed]:animate-out",
          "data-[state=open]:fade-in data-[state=closed]:fade-out",
          props.className,
        )}
      />
    </Div>
  );
};
