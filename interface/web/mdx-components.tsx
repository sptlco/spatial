// Copyright © Spatial Corporation. All rights reserved.

import { MDXComponents } from "mdx/types";

import { Paragraph } from "@sptlco/design";

/**
 * Use custom Markdown components.
 * @returns Custom components for use in Markdown.
 */
export function useMDXComponents(): MDXComponents {
  return {
    p: ({ children }) => <Paragraph className="xl:max-w-3xl">{children}</Paragraph>
  };
}
