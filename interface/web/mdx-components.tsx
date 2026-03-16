// Copyright © Spatial Corporation. All rights reserved.

import { MDXComponents } from "mdx/types";

import { H1, H2, Paragraph } from "@sptlco/design";

/**
 * Use custom Markdown components.
 * @returns Custom components for use in Markdown.
 */
export function useMDXComponents(): MDXComponents {
  return {
    h1: (props) => <H1 {...props} className="text-5xl xl:text-6xl" />,
    h2: (props) => <H2 {...props} className="text-4xl xl:text-5xl" />,
    p: (props) => <Paragraph {...props} />
  };
}
