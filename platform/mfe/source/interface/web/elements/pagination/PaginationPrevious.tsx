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
 * Create a new pagination previous element.
 * @param props Configurable options for the element.
 * @returns A pagination previous element.
 */
export const PaginationPrevious: Element<PaginationLinkProps> = (
  props: PaginationLinkProps,
): Node => {
  return (
    <PaginationLink className={clsx("gap-1/2u !pr-1u")} {...props}>
      <Icon icon="chevron_left" />
      <Span>Previous</Span>
    </PaginationLink>
  );
};
