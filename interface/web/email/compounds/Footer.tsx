// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";

import { Element, Link, Node, Section, Text } from "..";

/**
 * Create a new footer element.
 * @returns A footer element.
 */
export const Footer: Element = (): Node => {
  return (
    <Section className={clsx("my-2u", "text-s text-center")}>
      <Text className="text-foreground-quaternary !m-0">
        Built at Spatial in Seattle, WA
      </Text>
    </Section>
  );
};
