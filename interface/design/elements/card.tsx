// Copyright © Spatial Corporation. All rights reserved.

import { Container, createElement, Span } from "..";
import { clsx } from "clsx";

/**
 * An element that displays content in a structured format.
 */
export const Card = {
  /**
   * Contains all the parts of a card.
   */
  Root: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} className={clsx("h-fit flex flex-col", "gap-10", props.className)} />
  )),

  /**
   * Content displayed at the top of a card.
   */
  Header: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} className={clsx("grid grid-cols-[1fr_auto] grid-rows-[auto_auto] gap-x-10 gap-y-2.5", props.className)} />
  )),

  /**
   * An element that states the card's purpose.
   */
  Title: createElement<typeof Span>((props, ref) => (
    <Span {...props} ref={ref} className={clsx("col-start-1 row-start-1 truncate", props.className)} />
  )),

  /**
   * An element that describes the card's content.
   */
  Description: createElement<typeof Span>((props, ref) => (
    <Span {...props} ref={ref} className={clsx("col-start-1 row-start-2 col-span-2", props.className)} />
  )),

  /**
   * A container for card actions.
   */
  Gutter: createElement<typeof Container>((props, ref) => (
    <Container {...props} ref={ref} className={clsx("col-start-2 row-start-1 flex items-center gap-2.5 self-start", props.className)} />
  )),

  /**
   * The main content displayed in the card.
   */
  Content: createElement<typeof Container>((props, ref) => <Container {...props} ref={ref} />),

  /**
   * Content displayed at the bottom of a card.
   */
  Footer: createElement<typeof Container>((props, ref) => <Container {...props} ref={ref} />)
};
