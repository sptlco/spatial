// Copyright © Spatial. All rights reserved.

import { Div, Element, Icon, IconProps, Node } from "..";

/**
 * Create a new toast icon element.
 * @param props Configurable options for the element.
 * @returns An toast icon element.
 */
export const ToastIcon: Element<IconProps> = (props: IconProps): Node => {
  return (
    <Div
      style={props.style}
      className="relative flex items-center justify-center"
    >
      <Icon className="absolute animate-ping" icon={props.icon} />
      <Icon ref={props.ref} icon={props.icon} />
    </Div>
  );
};
