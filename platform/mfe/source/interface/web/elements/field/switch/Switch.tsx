// Copyright © Spatial Corporation. All rights reserved.

import * as Primitive from "@radix-ui/react-switch";
import { Element, Field, Node, SwitchProps } from "../..";
import clsx from "clsx";

/**
 * Create a new switch element.
 * @param props Configurable options for the element.
 * @returns A switch element.
 */
export const Switch: Element<SwitchProps> = (props: SwitchProps): Node => {
  return (
    <Field
      ref={props.ref}
      style={props.style}
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
    >
      <Primitive.Root
        name={props.name}
        checked={props.checked}
        onCheckedChange={props.onChange}
        disabled={props.disabled}
        className={clsx(
          "w-3u h-3/2u",
          "transition-all",
          "bg-background-quaternary rounded-full",
          "data-[state=checked]:bg-background-control-selected-default",
          "data-[disabled]:pointer-events-none data-[disabled]:cursor-not-allowed",
        )}
      >
        <Primitive.Thumb
          className={clsx(
            "size-3/2u flex",
            "transition-all will-change-transform",
            "bg-background-primary rounded-full",
            "scale-90 data-[state=checked]:translate-x-full",
          )}
        />
      </Primitive.Root>
    </Field>
  );
};
