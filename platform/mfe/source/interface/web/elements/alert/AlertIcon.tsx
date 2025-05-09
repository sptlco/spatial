// Copyright © Spatial. All rights reserved.

import { Div, Element, Icon, IconProps, Node } from "..";

/**
 * Create a new alert icon element.
 * @param props Configurable options for the element.
 * @returns An alert icon element.
 */
export const AlertIcon: Element<IconProps> = (props: IconProps): Node => {
  return (
    <Div
      style={props.style}
      className="size-5/2u relative flex items-center justify-center"
    >
      <Icon className="absolute animate-ping" icon={props.icon} />
      <Icon ref={props.ref} icon={props.icon} />
    </Div>
  );
};
