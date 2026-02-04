// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { FormEvent, useEffect, useState } from "react";
import { Button, createElement, Field, Form, Icon, LI, Span, UL } from "..";

/**
 * Configurable options for a pagination element.
 */
export type PaginationProps = {
  page: number;
  pages: number;
  onPageChange?: (page: number) => void;
};

/**
 * Pagination with page navigation, next and previous links.
 */
export const Pagination = createElement<typeof UL, PaginationProps>(({ onPageChange, ...props }, ref) => {
  const [page, setPage] = useState(props.page.toString());

  const valid = () => !!page && !isNaN(Number(page));

  const navigate = (e: FormEvent) => {
    e.preventDefault();

    if (!valid()) {
      return;
    }

    onPageChange?.(Math.min(Number(page), props.pages));
  };

  const reset = () => {
    if (!valid()) {
      setPage(props.page.toString());
    }
  };

  useEffect(() => {
    setPage(props.page.toString());
  }, [props.page]);

  return (
    <UL {...props} ref={ref} className={clsx("flex items-center gap-5", props.className)}>
      <LI className="flex">
        <Button intent="ghost" className="p-0! size-7!" disabled={props.page === 1} onClick={() => onPageChange?.(Math.max(props.page - 1, 1))}>
          <Icon symbol="chevron_left" />
        </Button>
      </LI>
      <Form className="flex w-10" onSubmit={navigate}>
        <Field
          type="text"
          className="text-center px-1.5!"
          value={page}
          disabled={props.pages === 1}
          onChange={(e) => setPage(e.target.value.trim())}
          onBlur={() => reset()}
        />
      </Form>
      <Span>of</Span>
      <Span>{props.pages}</Span>
      <LI className="flex">
        <Button
          intent="ghost"
          className="p-0! size-7!"
          disabled={props.page === props.pages}
          onClick={() => onPageChange?.(Math.min(props.page + 1, props.pages))}
        >
          <Icon symbol="chevron_right" />
        </Button>
      </LI>
    </UL>
  );
});
