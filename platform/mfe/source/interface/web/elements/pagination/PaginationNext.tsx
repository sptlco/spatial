// Copyright © Spatial. All rights reserved.

import clsx from "clsx";
import {
  Element,
  Icon,
  Node,
  PaginationLink,
  PaginationLinkProps,
  Span,
} from "..";

/**
 * Create a new pagination next element.
 * @param props Configurable options for the element.
 * @returns A pagination next element.
 */
export const PaginationNext: Element<PaginationLinkProps> = (
  props: PaginationLinkProps,
): Node => {
  return (
    <PaginationLink className={clsx("gap-1/2u !pl-1u")} {...props}>
      <Span>Next</Span>
      <Icon icon="chevron_right" />
    </PaginationLink>
  );
};
