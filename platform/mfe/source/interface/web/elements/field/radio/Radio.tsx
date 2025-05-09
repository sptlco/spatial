// Copyright © Spatial. All rights reserved.

import * as Primitive from "@radix-ui/react-radio-group";
import { Element, Field, Node, RadioProps } from "../..";
import { cva } from "class-variance-authority";
import clsx from "clsx";

const classes = cva(["flex w-full"], {
  variants: {
    orientation: {
      vertical: ["flex-col space-y-1/2u"],
      horizontal: ["flex-wrap space-x-1/2u"],
    },
  },
});

/**
 * Create a new radio element.
 * @param props Configurable options for the element.
 * @returns A radio element.
 */
export const Radio: Element<RadioProps> = ({
  orientation = "vertical",
  ...props
}: RadioProps): Node => {
  return (
    <Field
      ref={props.ref}
      style={props.style}
      className={props.className}
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
    >
      <Primitive.Root className="w-full">
        <Primitive.RadioGroup
          name={props.name}
          value={props.value}
          onValueChange={props.onChange}
          className={clsx(
            "data-[disabled]:pointer-events-none data-[disabled]:cursor-not-allowed",
            classes({ orientation }),
          )}
          children={props.children}
          disabled={props.disabled}
        />
      </Primitive.Root>
    </Field>
  );
};
