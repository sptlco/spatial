// Copyright © Spatial. All rights reserved.

import { clsx } from "clsx";

import { Element, Image, Node, Section } from "..";

/**
 * Create a new header element.
 * @returns A header element.
 */
export const Header: Element = (): Node => {
  return (
    <Section className={clsx("mt-4u mb-3u text-center")}>
      <Image
        src={`${process.env.NEXT_PUBLIC_BASE_URL}/assets/symbol.png`}
        className="h-1u mx-auto"
      />
    </Section>
  );
};
