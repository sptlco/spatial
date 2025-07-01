// Copyright © Spatial Corporation. All rights reserved.

import { CarouselOrientation, CarouselScrollDirection, ElementProps } from "..";

/**
 * Configurable options for a carousel element.
 */
export type CarouselProps = ElementProps & {
  /**
   * The carousel's orientation.
   */
  orientation?: CarouselOrientation;

  /**
   * Whether or not to loop the carousel.
   */
  loop?: boolean;

  /**
   * Whether or not the carousel should automatically play.
   */
  autoPlay?: boolean;

  /**
   * Whether or not the carousel should automatically scroll.
   */
  autoScroll?: boolean;

  /**
   * The direction the carousel automatically scrolls in.
   */
  direction?: CarouselScrollDirection;

  /**
   * Whether or not the carousel should stop automatically scrolling
   * when the user interacts with it.
   */
  stopOnInteraction?: boolean;

  /**
   * Whether or not the carousel should stop automatically scrolling
   * when the user hovers over it.
   */
  stopOnMouseEnter?: boolean;

  /**
   * Whether or not the carousel should stop automatically scrolling
   * when the user focuses on an element inside of it.
   */
  stopOnFocusIn?: boolean;

  /**
   * Whether or not to show navigation buttons.
   */
  showButtons?: boolean;
};
