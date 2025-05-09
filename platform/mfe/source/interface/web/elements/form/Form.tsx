// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import { Element, FormProps, Node } from "..";

/**
 * Create a new form element.
 * @param props Configurable options for the element.
 * @returns A form element.
 */
export const Form: Element<FormProps> = (props: FormProps): Node => {
  return (
    <form
      ref={props.ref}
      style={props.style}
      className={clsx("space-y-3/2u flex flex-col", props.className)}
      onSubmit={props.onSubmit}
      children={props.children}
    />
  );
};
