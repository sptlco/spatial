// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { Fragment, useState } from "react";
import { usePathname, useRouter, useSearchParams } from "next/navigation";

import { Button, createElement, Dialog, Field, Form, Icon } from "@sptlco/design";

export const Search = createElement<typeof Fragment>(() => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [open, setOpen] = useState(false);
  const [search, setSearch] = useState(() => searchParams.get("keywords") ?? "");

  const commit = (value: string) => {
    const params = new URLSearchParams(searchParams.toString());
    const next = value.trim();

    if (next) {
      params.set("keywords", next);
    } else {
      params.delete("keywords");
    }

    params.delete("page");
    router.replace(`${pathname}?${params.toString()}`, { scroll: false });

    setOpen(false);
  };

  const onOpenChange = (next: boolean) => {
    if (next) {
      setSearch(searchParams.get("keywords") ?? "");
    }

    setOpen(next);
  };

  const active = !!searchParams.get("keywords");

  return (
    <Dialog.Root open={open} onOpenChange={onOpenChange}>
      <Dialog.Trigger asChild>
        <Button intent="none" size="fit" className="p-2">
          <Icon symbol="search" className={active ? "fill" : undefined} />
        </Button>
      </Dialog.Trigger>
      <Dialog.Content className="p-0! bg-transparent!" closeButton={false}>
        <Form
          className="group flex relative w-full max-w-md gap-4 items-end"
          onSubmit={(e) => {
            e.preventDefault();
            commit(search);
          }}
        >
          <Icon symbol="search" size={20} className="absolute left-2.5 bottom-2.5" />
          <Field
            type="text"
            placeholder="What are you looking for?"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Escape") {
                e.preventDefault();

                commit("");
                setSearch("");
              }
            }}
            className="pl-10 pr-12"
            autoFocus
          />
          {search ? (
            <Button
              type="button"
              className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5")}
              onClick={() => {
                commit("");
                setSearch("");
              }}
            >
              <Icon symbol="close" size={20} />
            </Button>
          ) : (
            <Button type="submit" className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5")}>
              <Icon symbol="arrow_right_alt" size={20} />
            </Button>
          )}
        </Form>
      </Dialog.Content>
    </Dialog.Root>
  );
});
