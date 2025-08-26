// Copyright © Spatial Corporation. All rights reserved.

import clsx from "clsx";
import { MDXComponents } from "mdx/types";

import {
  A,
  H1,
  H2,
  H3,
  H4,
  H5,
  H6,
  P,
  ScrollArea,
  UL,
} from "@spatial/elements";

/**
 * Get a list of MDX components.
 * @param components A list of MDX components.
 * @returns A list of MDX components.
 */
export function useMDXComponents(components: MDXComponents): MDXComponents {
  return {
    h1: ({ children }) => (
      <H1 className="my-1u text-4xl font-bold">{children}</H1>
    ),
    h2: ({ children }) => (
      <H2 className="my-1u text-3xl font-bold">{children}</H2>
    ),
    h3: ({ children }) => (
      <H3 className="my-1u text-2xl font-bold">{children}</H3>
    ),
    h4: ({ children }) => (
      <H4 className="my-1u text-xl font-bold">{children}</H4>
    ),
    h5: ({ children }) => (
      <H5 className="text-l my-1u font-bold">{children}</H5>
    ),
    h6: ({ children }) => (
      <H6 className="text-m my-1u font-bold">{children}</H6>
    ),

    p: (props) => <P className="my-1u">{props.children}</P>,

    a: (props) => (
      <A href={props.href} highlight external>
        {props.children}
      </A>
    ),

    ul: (props) => <UL className="pl-2u list-disc">{props.children}</UL>,

    pre: (props) => (
      <pre className={clsx("rounded-1/2u bg-background-secondary p-1u w-full")}>
        <ScrollArea className="w-full" orientation="horizontal">
          {props.children}
        </ScrollArea>
      </pre>
    ),

    ...components,
  };
}
