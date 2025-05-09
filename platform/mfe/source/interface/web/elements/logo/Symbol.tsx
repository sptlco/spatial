// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";
import { Element, ElementProps, Node } from "..";

/**
 * Create a new symbol element.
 * @returns A symbol element.
 */
export const Symbol: Element = (props: ElementProps): Node => {
  return (
    <svg
      className={clsx(props.className)}
      viewBox="0 0 48 32"
      fill="none"
      xmlns="http://www.w3.org/2000/svg"
    >
      <path
        d="M47.2136 10.9667H36.4069L23.2837 23.3792C22.7441 23.89 21.6358 24.3075 20.8208 24.3075H12.3592L5.19915 31.0789C4.67502 31.5764 4.89267 31.9828 5.68552 31.9828H24.9938C25.8 31.9828 26.8971 31.5697 27.4323 31.0634L47.7089 11.8839C48.2441 11.3776 48.0198 10.9645 47.2136 10.9645V10.9667Z"
        fill="currentColor"
      />
      <path
        d="M22.4755 20.0989L42.7521 0.919442C43.2873 0.413082 43.063 0 42.2568 0H23.0063C22.2001 0 21.103 0.413082 20.5678 0.919442L0.29117 20.0989C-0.244061 20.6053 -0.0197527 21.0184 0.786425 21.0184H20.037C20.8431 21.0184 21.9403 20.6053 22.4755 20.0989Z"
        fill="currentColor"
      />
    </svg>
  );
};
