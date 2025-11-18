// Copyright © Spatial Corporation. All rights reserved.

import { ComponentProps, ComponentRef, ElementType, forwardRef, ForwardRefRenderFunction, PropsWithoutRef } from "react";

/**
 * A function that renders a design element.
 */
export type Renderer<P, T extends ElementType> = ForwardRefRenderFunction<ComponentRef<T>, PropsWithoutRef<P & ComponentProps<T>>>;

/**
 * Create a new design element.
 */
export const createElement = <P = {}, T extends ElementType = "div">(render: Renderer<P, T>) => forwardRef(render);
