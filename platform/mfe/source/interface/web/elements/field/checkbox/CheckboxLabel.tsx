// Copyright © Spatial. All rights reserved.

import { Label as UILabel } from "@headlessui/react";
import { Element, ElementProps, Node } from "../..";
import clsx from "clsx";

/**
 * Create a new checkbox label element.
 * @param props Configurable options for the element.
 * @returns A checkbox label element.
 */
export const CheckboxLabel: Element = (props: ElementProps): Node => {
  return (
    <UILabel
      ref={props.ref}
      style={props.style}
      className={clsx("cursor-pointer", props.className)}
      children={props.children}
    />
  );
};
