// Copyright © Spatial Corporation. All rights reserved.

import { createElement, Icon, Span } from "..";
import { clsx } from "clsx";
import Primitive, { type LinkProps } from "next/link";

/**
 * A hyperlink, which is used to link from one page to another.
 */
export const Link = createElement<typeof Primitive, LinkProps>((props, ref) => (
  <Primitive
    {...props}
    href={props.href || "#"}
    ref={ref}
    className={clsx(
      "cursor-pointer inline-flex",
      "text-button-primary hover:text-button-primary-hover active:text-button-primary-active font-medium",
      "transition-all",
      props.className
    )}
  >
    {props.children}
    {props.target == "_blank" && (
      <Span className="inline-flex items-center align-middle">
        <Icon className="ml-0.5 font-medium text-[1em]!" symbol="arrow_outward" />
      </Span>
    )}
  </Primitive>
));
