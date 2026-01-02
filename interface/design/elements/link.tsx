// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { createElement, Icon, resolve, Span } from "..";

/**
 * A hyperlink, which is used to link from one page to another.
 */
export const Link = createElement<"a">((props, ref) => (
  <>
    <a
      {...props}
      ref={ref}
      className={clsx(
        "cursor-pointer inline-flex",
        "text-button-primary hover:text-button-primary-hover active:text-button-primary-active font-medium",
        "transition-all",
        props.className
      )}
    >
      <Span className="inline-flex">{props.children}</Span>
      {props.target == "_blank" && (
        <Span className="inline-flex items-center align-middle">
          <Icon className="ml-0.5 font-medium text-[1em]!" symbol="arrow_outward" />
        </Span>
      )}
    </a>
  </>
));
