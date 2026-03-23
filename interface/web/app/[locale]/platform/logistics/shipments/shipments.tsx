// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { Fragment, useEffect, useMemo, useState } from "react";

import {
  Accordion,
  Button,
  Card,
  Container,
  createElement,
  Dropdown,
  Field,
  Form,
  H1,
  Icon,
  LI,
  Pagination,
  Separator,
  Sheet,
  Span,
  UL
} from "@sptlco/design";

import { View, views } from "./view";
import { Filters } from "./filters";
import { Sort } from "./sort";

type Address = {
  name?: string;
  company?: string;
  line1: string;
  line2?: string;
  city: string;
  state: string;
  zip: string;
  country: string;
};

type Shipment = {
  id: string;
  from: Address;
  to: Address;
};

export const Shipments = createElement<typeof Card.Root>((props, ref) => {
  const shipments: Shipment[] = [];

  for (let i = 0; i < 15; i++) {
    shipments.push({
      id: `SHP-${i}`,
      from: {
        company: "Spatial Corporation",
        line1: "240 2ND AVE S",
        line2: "STE 201K",
        city: "Seattle",
        state: "WA",
        zip: "98104",
        country: "USA"
      },
      to: {
        name: "Dakarai Cundiff",
        company: "Spatial Corporation",
        line1: "2014 FAIRVIEW AVE",
        line2: "APT 2211",
        city: "Seattle",
        state: "WA",
        zip: "98121",
        country: "USA"
      }
    });
  }

  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [search, setSearch] = useState("");
  const [view, setView] = useState(views[0].name);
  const [creating, setCreating] = useState(false);
  const [edit, setEdit] = useState<Shipment>();

  const keywords =
    searchParams
      .get("keywords")
      ?.split(",")
      .map((k) => k.trim())
      .filter(Boolean) ?? [];

  const commitKeywords = (value: string) => {
    const params = new URLSearchParams(searchParams.toString());

    const next = value
      .split(/\s+/)
      .map((k) => k.trim())
      .filter(Boolean);

    if (next.length > 0) {
      params.set("keywords", next.join(","));
      params.delete("page");
    } else {
      params.delete("keywords");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const clearKeywords = () => {
    const params = new URLSearchParams(searchParams.toString());

    params.delete("keywords");
    params.delete("page");

    setSearch("");

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const navigate = (page: number) => {
    const params = new URLSearchParams(searchParams.toString());

    if (page > 1) {
      params.set("page", page.toString());
    } else {
      params.delete("page");
    }

    router.replace(`${pathname}?${params.toString()}`, { scroll: false });
  };

  const PAGE_SIZE = 15;

  const pages = 1;
  const page = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  useEffect(() => {
    setSearch(keywords.join(" "));
  }, [searchParams]);

  const render = () => {
    switch (view) {
      case "grid": {
        const columns = [0, 1, 2].map((col) => shipments.filter((_, i) => i % 3 === col));
        const twoColumns = [0, 1].map((col) => shipments.filter((_, i) => i % 2 === col));

        const jsx = (shipment: Shipment, i: number) => <Shipment.Card key={i} shipment={shipment} onUpdate={() => setEdit(shipment)} />;

        return (
          <>
            {shipments.map((shipment, i) => (
              <Shipment.Editor
                key={i}
                open={edit?.id === shipment.id}
                onOpenChange={(open) => setEdit(open ? shipment : undefined)}
                shipment={shipment}
                onEdit={(_) => {}}
              />
            ))}

            {/* Mobile: single column */}
            <UL className="flex flex-col gap-10 sm:hidden">{shipments.map(jsx)}</UL>

            {/* Tablet: 2 independent columns */}
            <Container className="hidden sm:flex xl:hidden gap-10">
              {twoColumns.map((col, ci) => (
                <UL key={ci} className="flex flex-col gap-10 flex-1">
                  {col.map(jsx)}
                </UL>
              ))}
            </Container>

            {/* Desktop: 3 independent columns */}
            <Container className="hidden xl:flex gap-10">
              {columns.map((col, ci) => (
                <UL key={ci} className="flex flex-col gap-10 flex-1">
                  {col.map(jsx)}
                </UL>
              ))}
            </Container>
          </>
        );
      }
      case "list": {
        return null;
      }
    }
  };

  return (
    <Card.Root {...props} ref={ref}>
      <Card.Content className="flex flex-col w-full px-10 xl:p-0 gap-10 xl:gap-20">
        <Container className="flex items-center justify-between">
          <H1 className="text-2xl font-extrabold">Shipments</H1>
          <Dropdown.Root>
            <Dropdown.Trigger asChild>
              <Button intent="ghost" className="xl:hidden size-10! p-0! data-[state=open]:bg-button-ghost-active">
                <Icon symbol="keyboard_arrow_down" />
              </Button>
            </Dropdown.Trigger>
            <Dropdown.Content>
              <Dropdown.Item onSelect={() => setCreating(true)}>
                <Icon symbol="add" />
                <Span>Create</Span>
              </Dropdown.Item>
            </Dropdown.Content>
          </Dropdown.Root>
          <Button className="hidden xl:flex" onClick={() => setCreating(true)}>
            <Icon symbol="add" />
            <Span>Create</Span>
          </Button>
        </Container>
        <Container className="flex flex-col sm:flex-row gap-2">
          <Form
            className="group relative w-full sm:max-w-sm flex items-center"
            onSubmit={(e) => {
              e.preventDefault();
              commitKeywords(search);
            }}
          >
            <Field
              type="text"
              id="search"
              name="search"
              placeholder="Search shipments"
              value={search}
              className="w-full pl-12 pr-12"
              onChange={(e) => setSearch(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Escape" && search.length > 0) {
                  e.preventDefault();
                  clearKeywords();
                }
              }}
            />
            <Icon symbol="search" className="absolute left-3" />
            {keywords.length > 0 ? (
              <Button
                type="button"
                onClick={clearKeywords}
                className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5", "group-focus-within:bg-button-highlight!")}
              >
                <Icon symbol="close" />
              </Button>
            ) : (
              <Button
                type="submit"
                className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5", "group-focus-within:bg-button-highlight!")}
              >
                <Icon symbol="arrow_right_alt" size={20} />
              </Button>
            )}
          </Form>
          <Container className="flex w-full sm:grow items-center justify-between gap-2">
            <Container className="flex items-center">
              <Filters />
              <Sort />
            </Container>
            <View
              type="single"
              value={view}
              onValueChange={(value) => {
                if (value) setView(value);
              }}
            />
          </Container>
        </Container>
        {render()}
        <Pagination pages={pages} page={page} className="self-center" onPageChange={navigate} />
      </Card.Content>
    </Card.Root>
  );
});

const Shipment = {
  Address: createElement<typeof Container, { title: string; address: Address }>(({ title, address, ...props }, ref) => {
    return (
      <Container {...props} ref={ref} className="flex flex-col p-10 gap-5">
        <Span className="flex items-center gap-2.5 text-lg xl:text-2xl font-bold">
          <Span>{title}</Span>
          <Span className="text-foreground-quaternary font-normal">
            {address.city}, {address.state}
          </Span>
        </Span>
        <UL className="flex flex-wrap gap-5 xl:gap-10 w-full">
          {address.name && (
            <LI className="flex flex-col mr-auto">
              <Span className="text-sm mb-2 text-foreground-quaternary">Name</Span>
              <Span className="truncate">{address.name}</Span>
            </LI>
          )}
          {address.company && (
            <LI className="flex flex-col mr-auto">
              <Span className="text-sm mb-2 text-foreground-quaternary">Company</Span>
              <Span className="truncate">{address.company}</Span>
            </LI>
          )}
          <LI className="flex flex-col col-start-3">
            <Span className="text-sm mb-2 text-foreground-quaternary">Address</Span>
            <Span className="truncate">{address.line1}</Span>
            {address.line2 && <Span className="truncate">{address.line2}</Span>}
            <Span className="truncate">
              {address.city}, {address.state} {address.zip}
            </Span>
          </LI>
        </UL>
      </Container>
    );
  }),

  Separator: createElement<typeof Separator>((props, ref) => <Separator {...props} ref={ref} className="flex w-full h-px bg-line-faint" />),

  Card: createElement<typeof Fragment, { shipment: Shipment; onUpdate?: () => void; onCancel?: () => void }>(
    ({ shipment, onUpdate, onCancel }, _) => {
      return (
        <Accordion.Root collapsible type="single" className="flex flex-col bg-input outline outline-line-faint rounded-4xl">
          <Accordion.Item value={shipment.id}>
            <Accordion.Header>
              <Accordion.Trigger className="group flex items-center w-full p-10">
                <Container className="flex flex-col items-start grow xl:gap-2">
                  <Container className="flex w-full items-center gap-2 justify-between">
                    <Span className="text-2xl xl:text-4xl font-bold">{shipment.id}</Span>
                    <Icon symbol="keyboard_arrow_down" className="font-light transition group-data-[state=open]:rotate-180 duration-300" />
                  </Container>
                  <Span className="text-foreground-quaternary text-xs uppercase">
                    {shipment.to.city}, {shipment.to.state} • 2 Parcels • 2 Units
                  </Span>
                </Container>
              </Accordion.Trigger>
            </Accordion.Header>
            <Accordion.Content>
              <Shipment.Address title="Origin" address={shipment.from} />
              <Shipment.Separator />
              <Shipment.Address title="Destination" address={shipment.to} />
              <Container className="flex flex-col items-center gap-2.5 p-4">
                <Button shape="pill" size="fill" onClick={onUpdate}>
                  <Span>Update</Span>
                </Button>
                <Button shape="pill" size="fill" onClick={onCancel} destructive>
                  Cancel
                </Button>
              </Container>
            </Accordion.Content>
          </Accordion.Item>
        </Accordion.Root>
      );
    }
  ),

  Section: createElement<typeof Accordion.Item, { label: string }>(({ label, children, ...props }, ref) => {
    return (
      <Accordion.Item {...props} ref={ref}>
        <Accordion.Header>
          <Accordion.Trigger
            className={clsx(
              "group flex items-center w-full py-2.5",
              "text-hint text-xs uppercase font-bold relative",
              "after:absolute after:w-[calc(100%+80px)] after:h-full after:bg-background-highlight/30 after:-left-10 after:-z-10"
            )}
          >
            <Span className="grow text-left">{label}</Span>
            <Icon symbol="keyboard_arrow_down" className="font-light transition group-data-[state=open]:rotate-180 duration-300" />
          </Accordion.Trigger>
        </Accordion.Header>
        <Accordion.Content>
          <Container className="grid grid-cols-1 sm:grid-cols-2 gap-10 py-10">{children}</Container>
        </Accordion.Content>
      </Accordion.Item>
    );
  }),

  Editor: createElement<typeof Sheet.Root, { shipment: Shipment; onEdit: (shipment: Shipment) => void }>(({ shipment, onEdit, ...props }, ref) => {
    const [from, setFrom] = useState<Address>({ ...shipment.from });
    const [to, setTo] = useState<Address>({ ...shipment.to });

    useEffect(() => {
      setFrom({ ...shipment.from });
      setTo({ ...shipment.to });
    }, [shipment]);

    const fromField = (key: keyof Address) => ({
      value: from[key] ?? "",
      onChange: (e: React.ChangeEvent<HTMLInputElement>) => setFrom((prev) => ({ ...prev, [key]: e.target.value }))
    });

    const toField = (key: keyof Address) => ({
      value: to[key] ?? "",
      onChange: (e: React.ChangeEvent<HTMLInputElement>) => setTo((prev) => ({ ...prev, [key]: e.target.value }))
    });

    const submit = (e: React.FormEvent) => {
      e.preventDefault();

      onEdit({ ...shipment, from, to });
    };

    return (
      <Sheet.Root {...props} ref={ref}>
        <Sheet.Content title={shipment.id} closeButton>
          <Form className="flex flex-col gap-10 sm:min-w-lg" onSubmit={submit}>
            <Accordion.Root type="multiple" defaultValue={["from", "to", "payload", "status"]} className="flex flex-col">
              <Shipment.Section value="from" label="Origin">
                <Field type="text" inset={false} label="Name" placeholder="Full name" {...fromField("name")} />
                <Field type="text" inset={false} label="Company" placeholder="Company" {...fromField("company")} />
                <Field type="text" inset={false} label="Address Line 1" placeholder="Address line 1" {...fromField("line1")} />
                <Field type="text" inset={false} label="Address Line 2" placeholder="Address line 2" {...fromField("line2")} />
                <Field type="text" inset={false} label="City" placeholder="City" {...fromField("city")} />
                <Field type="text" inset={false} label="State" placeholder="State" {...fromField("state")} />
                <Field type="text" inset={false} label="ZIP" placeholder="ZIP" {...fromField("zip")} />
              </Shipment.Section>

              <Shipment.Section value="to" label="Destination">
                <Field type="text" inset={false} label="Name" placeholder="Full name" {...toField("name")} />
                <Field type="text" inset={false} label="Company" placeholder="Company" {...toField("company")} />
                <Field type="text" inset={false} label="Address Line 1" placeholder="Address line 1" {...toField("line1")} />
                <Field type="text" inset={false} label="Address Line 2" placeholder="Address line 2" {...toField("line2")} />
                <Field type="text" inset={false} label="City" placeholder="City" {...toField("city")} />
                <Field type="text" inset={false} label="State" placeholder="State" {...toField("state")} />
                <Field type="text" inset={false} label="ZIP" placeholder="ZIP" {...toField("zip")} />
              </Shipment.Section>

              <Shipment.Section value="payload" label="Payload"></Shipment.Section>

              <Shipment.Section value="status" label="Status"></Shipment.Section>
            </Accordion.Root>

            <Container className="flex gap-4">
              <Button type="submit">Update</Button>
              <Sheet.Close asChild>
                <Button intent="ghost">Cancel</Button>
              </Sheet.Close>
            </Container>
          </Form>
        </Sheet.Content>
      </Sheet.Root>
    );
  })
};
