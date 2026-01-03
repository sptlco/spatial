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
      "transition-all duration-500 ease-out",
      "group-has-[*[data-slot=dialog-content][data-state=open]]/body:scale-[0.97]",
      "group-has-[*[data-slot=dialog-content][data-state=open]]/body:rounded-4xl",
      "group-has-[*[data-slot=dialog-content][data-state=open]]/body:overflow-hidden",
      "group-has-[*[data-slot=sheet-content][data-state=open]]/body:scale-[0.97]",
      "group-has-[*[data-slot=sheet-content][data-state=open]]/body:rounded-4xl",
      "group-has-[*[data-slot=sheet-content][data-state=open]]/body:overflow-hidden",
      props.className
    )}
  />
));
