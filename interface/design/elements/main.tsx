// Copyright © Spatial Corporation. All rights reserved.

import { createElement } from "..";
import { clsx } from "clsx";

/**
 * The dominant content of a document.
 */
export const Main = createElement<"main">((props, ref) => (
  <main
    {...props}
    ref={ref}
    className={clsx(
      "transition-transform duration-500 ease-out",
      "group-has-[*[data-slot=dialog-content][data-state=open]]/body:scale-[0.95]",
      "group-has-[*[data-slot=sheet-content][data-state=open]]/body:scale-[0.95]",
      props.className
    )}
  />
));
