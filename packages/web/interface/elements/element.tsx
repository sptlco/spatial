// Copyright © Spatial Corporation. All rights reserved.

import {
  ComponentProps,
  ComponentRef,
  ElementType,
  forwardRef,
  ForwardRefExoticComponent,
  ForwardRefRenderFunction,
  PropsWithoutRef,
  RefAttributes
} from "react";

/**
 * A function that renders a design element.
 */
export type Renderer<T extends ElementType, P> = ForwardRefRenderFunction<ComponentRef<T>, PropsWithoutRef<P & ComponentProps<T>>>;

/**
 * A design element.
 */
export type Element<T extends ElementType, P> = ForwardRefExoticComponent<PropsWithoutRef<P & ComponentProps<T>> & RefAttributes<ComponentRef<T>>>;

/**
 * Create a new design element.
 * @param render A function that will be called to render the element.
 * @returns A design element.
 */
export const createElement = <T extends ElementType, P = {}>(render: Renderer<T, P>) => forwardRef(render);
