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
export type Renderer<P, T extends ElementType> = ForwardRefRenderFunction<ComponentRef<T>, PropsWithoutRef<P & ComponentProps<T>>>;

/**
 * A design element.
 */
export type Element<P, T extends ElementType> = ForwardRefExoticComponent<PropsWithoutRef<P & ComponentProps<T>> & RefAttributes<ComponentRef<T>>>;

/**
 * Create a new design element.
 * @param render A function that will be called to render the element.
 */
export function createElement<T extends ElementType = "div">(render: Renderer<{}, T>): Element<{}, T>;

/**
 * Create a new design element.
 * @param render A function that will be called to render the element.
 */
export function createElement<P = {}, T extends ElementType = "div">(render: Renderer<P, T>): Element<P, T>;

/**
 * Create a new design element.
 * @param render A function that will be called to render the element.
 * @returns A design element.
 */
export function createElement<P = {}, T extends ElementType = "div">(render: Renderer<P, T>) {
  return forwardRef(render);
}
