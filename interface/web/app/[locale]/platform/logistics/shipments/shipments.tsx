// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { clsx } from "clsx";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import { Fragment, useEffect, useMemo, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import useSWR from "swr";

import { Shipment as ShipmentType, Address } from "@sptlco/data";
import { Spatial } from "@sptlco/client";

import {
  Accordion,
  Button,
  Card,
  Container,
  createElement,
  Dialog,
  Dropdown,
  Field,
  Form,
  H1,
  Icon,
  LI,
  Pagination,
  Paragraph,
  Separator,
  Sheet,
  Span,
  Table,
  toast,
  UL
} from "@sptlco/design";

import { View, views } from "./view";
import { Filters } from "./filters";
import { Sort } from "./sort";

// ─── Animation variants ────────────────────────────────────────────────────

const gridContainerVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: {
      staggerChildren: 0.06,
      delayChildren: 0.05
    }
  },
  exit: { opacity: 0, transition: { duration: 0.15 } }
};

const columnVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: {
      staggerChildren: 0.08
    }
  }
};

const cardVariants = {
  hidden: { opacity: 0, y: 20, scale: 0.98 },
  visible: {
    opacity: 1,
    y: 0,
    scale: 1,
    transition: {
      type: "spring",
      stiffness: 260,
      damping: 22
    }
  }
};

const tableVariants = {
  hidden: { opacity: 0 },
  visible: {
    opacity: 1,
    transition: {
      staggerChildren: 0.04,
      delayChildren: 0.05
    }
  },
  exit: { opacity: 0, transition: { duration: 0.15 } }
};

const rowVariants = {
  hidden: { opacity: 0, x: -8 },
  visible: {
    opacity: 1,
    x: 0,
    transition: {
      type: "spring",
      stiffness: 300,
      damping: 28
    }
  }
};

// ─── Animated wrappers ─────────────────────────────────────────────────────

const MotionUL = motion(UL as any);
const MotionLI = motion(LI as any);
const MotionContainer = motion(Container as any);
const MotionTBody = motion(Table.Body as any);
const MotionTRow = motion(Table.Row as any);

export const Shipments = createElement<typeof Card.Root>((props, ref) => {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [search, setSearch] = useState("");
  const [view, setView] = useState(views[0].name);
  const [creating, setCreating] = useState(false);
  const [edit, setEdit] = useState<ShipmentType>();

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
  const page = useMemo(() => Math.max(1, Number(searchParams.get("page") ?? 1)), [searchParams]);

  const shipments = useSWR("shipments/list", async () => {
    const response = await Spatial.shipments.list();

    if (response.error) {
      throw response.error;
    }

    return response.data;
  });

  const allData = shipments.data ?? [];

  const filteredData = useMemo(() => {
    if (keywords.length === 0) return allData;

    return allData.filter((s) => {
      const haystack = [s.id, s.from.city, s.from.state, s.from.company, s.from.name, s.to.city, s.to.state, s.to.company, s.to.name]
        .filter(Boolean)
        .join(" ")
        .toLowerCase();

      return keywords.some((k) => haystack.includes(k.toLowerCase()));
    });
  }, [allData, keywords]);

  const paginatedData = useMemo(() => {
    const start = (page - 1) * PAGE_SIZE;
    return filteredData.slice(start, start + PAGE_SIZE);
  }, [filteredData, page]);

  const pages = Math.ceil(filteredData.length / PAGE_SIZE);

  useEffect(() => {
    setSearch(keywords.join(" "));
  }, [searchParams]);

  const cancel = (shipment: ShipmentType) =>
    toast.promise(Spatial.shipments.del(shipment.id), {
      loading: "Cancelling shipment…",
      success: async () => {
        await shipments.mutate();
        return { message: "Shipment cancelled", description: shipment.id };
      },
      error: { message: "Cancel failed", description: "Something went wrong." }
    });

  const card = (shipment: ShipmentType, i: number) => (
    <MotionLI
      key={shipment.id}
      variants={cardVariants}
      whileHover={{ y: -4, transition: { type: "spring", stiffness: 400, damping: 20 } }}
      whileTap={{ scale: 0.985, transition: { type: "spring", stiffness: 500, damping: 25 } }}
      layout
    >
      <Shipment.Card shipment={shipment} onUpdate={() => setEdit(shipment)} onCancel={() => cancel(shipment)} />
    </MotionLI>
  );

  const renderSkeleton = () => (
    <UL className="flex flex-col gap-10">
      {[...Array(PAGE_SIZE)].map((_, i) => (
        <LI key={i} className="rounded-4xl animate-pulse bg-input h-24" />
      ))}
    </UL>
  );

  const renderTableSkeleton = () => (
    <>
      {[...Array(PAGE_SIZE)].map((_, i) => (
        <Table.Row key={i}>
          <Table.Cell>
            <Span className="flex w-32 h-4 rounded-full animate-pulse bg-input" />
          </Table.Cell>
          <Table.Cell className="hidden sm:table-cell">
            <Container className="flex flex-col gap-2">
              <Span className="w-24 h-4 rounded-full animate-pulse bg-input" />
              <Span className="w-16 h-3 rounded-full animate-pulse bg-input" />
            </Container>
          </Table.Cell>
          <Table.Cell className="hidden sm:table-cell">
            <Container className="flex flex-col gap-2">
              <Span className="w-24 h-4 rounded-full animate-pulse bg-input" />
              <Span className="w-16 h-3 rounded-full animate-pulse bg-input" />
            </Container>
          </Table.Cell>
          <Table.Cell />
        </Table.Row>
      ))}
    </>
  );

  const render = () => {
    switch (view) {
      case "grid": {
        if (shipments.isLoading || !shipments.data) {
          return renderSkeleton();
        }

        const columns = [0, 1, 2].map((col) => paginatedData.filter((_, i) => i % 3 === col));
        const twoColumns = [0, 1].map((col) => paginatedData.filter((_, i) => i % 2 === col));

        return (
          <AnimatePresence mode="wait">
            <motion.div key="grid-view" variants={gridContainerVariants} initial="hidden" animate="visible" exit="exit">
              {paginatedData.map((shipment) => (
                <Shipment.Editor
                  key={shipment.id}
                  open={edit?.id === shipment.id}
                  onOpenChange={(open) => setEdit(open ? shipment : undefined)}
                  shipment={shipment}
                  onEdit={async (_) => {
                    await shipments.mutate();
                    setEdit(undefined);
                  }}
                />
              ))}

              {/* Mobile: single column */}
              <MotionUL className="flex flex-col gap-10 sm:hidden" variants={columnVariants}>
                {paginatedData.map(card)}
              </MotionUL>

              {/* Tablet: 2 columns */}
              <MotionContainer className="hidden sm:flex xl:hidden gap-10" variants={gridContainerVariants}>
                {twoColumns.map((col, ci) => (
                  <MotionUL key={ci} className="flex flex-col gap-10 flex-1" variants={columnVariants}>
                    {col.map(card)}
                  </MotionUL>
                ))}
              </MotionContainer>

              {/* Desktop: 3 columns */}
              <MotionContainer className="hidden xl:flex gap-10" variants={gridContainerVariants}>
                {columns.map((col, ci) => (
                  <MotionUL key={ci} className="flex flex-col gap-10 flex-1" variants={columnVariants}>
                    {col.map(card)}
                  </MotionUL>
                ))}
              </MotionContainer>
            </motion.div>
          </AnimatePresence>
        );
      }

      case "list": {
        return (
          <AnimatePresence mode="wait">
            <motion.div key="list-view" variants={tableVariants} initial="hidden" animate="visible" exit="exit">
              {paginatedData.map((shipment) => (
                <Shipment.Editor
                  key={shipment.id}
                  open={edit?.id === shipment.id}
                  onOpenChange={(open) => setEdit(open ? shipment : undefined)}
                  shipment={shipment}
                  onEdit={async (_) => {
                    await shipments.mutate();
                    setEdit(undefined);
                  }}
                />
              ))}

              <Table.Root className="w-full table-fixed border-separate border-spacing-y-4">
                <Table.Header>
                  <Table.Row>
                    <Table.Column className="text-left w-40">
                      <Span className="text-sm text-foreground-quaternary font-semibold">Shipment</Span>
                    </Table.Column>
                    <Table.Column className="text-left hidden sm:table-cell">
                      <Span className="text-sm text-foreground-quaternary font-semibold">Origin</Span>
                    </Table.Column>
                    <Table.Column className="text-left hidden sm:table-cell">
                      <Span className="text-sm text-foreground-quaternary font-semibold">Destination</Span>
                    </Table.Column>
                    <Table.Column className="w-12 xl:w-16" />
                  </Table.Row>
                </Table.Header>
                <MotionTBody variants={tableVariants}>
                  {shipments.isLoading || !shipments.data
                    ? renderTableSkeleton()
                    : paginatedData.map((shipment) => (
                        <Shipment.Row key={shipment.id} shipment={shipment} onUpdate={() => setEdit(shipment)} onCancel={() => cancel(shipment)} />
                      ))}
                </MotionTBody>
              </Table.Root>
            </motion.div>
          </AnimatePresence>
        );
      }
    }
  };

  return (
    <Card.Root {...props} ref={ref}>
      <Shipment.Creator
        open={creating}
        onOpenChange={setCreating}
        onCreate={async (_) => {
          await shipments.mutate();
          setCreating(false);
        }}
      />
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
      <Container {...props} ref={ref} className={clsx("flex flex-col p-10 gap-10", props.className)}>
        <Span className="flex flex-col items-start text-sm xl:text-lg font-extrabold">
          <Span>{title}</Span>
          <Span className="font-normal text-foreground-quaternary normal-case">
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

  Row: createElement<typeof Fragment, { shipment: ShipmentType; onUpdate?: () => void; onCancel?: () => void }>(
    ({ shipment, onUpdate, onCancel }, _) => {
      const [cancelling, setCancelling] = useState(false);

      return (
        <>
          <MotionTRow variants={rowVariants} className="cursor-pointer rounded-2xl">
            <Table.Cell onClick={onUpdate}>
              <Container className="flex flex-col gap-1 py-2.5 xl:py-5">
                <Span className="font-bold uppercase">{shipment.id.slice(-8)}</Span>
                <Span className="text-xs inline-flex gap-2 items-center text-foreground-quaternary sm:hidden">
                  <Span>{shipment.from.city}</Span>
                  <Icon symbol="arrow_right_alt" size={16} />
                  <Span>{shipment.to.city}</Span>
                </Span>
              </Container>
            </Table.Cell>
            <Table.Cell className="hidden sm:table-cell" onClick={onUpdate}>
              <Container className="flex flex-col gap-0.5">
                <Span className="font-medium truncate">{shipment.from.company ?? shipment.from.name ?? "—"}</Span>
                <Span className="text-sm text-foreground-quaternary">
                  {shipment.from.city}, {shipment.from.state}
                </Span>
              </Container>
            </Table.Cell>
            <Table.Cell className="hidden sm:table-cell" onClick={onUpdate}>
              <Container className="flex flex-col gap-0.5">
                <Span className="font-medium truncate">{shipment.to.company ?? shipment.to.name ?? "—"}</Span>
                <Span className="text-sm text-foreground-quaternary">
                  {shipment.to.city}, {shipment.to.state}
                </Span>
              </Container>
            </Table.Cell>
            <Table.Cell>
              <Dropdown.Root>
                <Dropdown.Trigger asChild>
                  <Button intent="ghost" className="ml-auto! size-10! p-0! data-[state=open]:bg-button-ghost-active">
                    <Icon symbol="more_vert" />
                  </Button>
                </Dropdown.Trigger>
                <Dropdown.Content align="end">
                  <Dropdown.Item onSelect={onUpdate}>
                    <Dropdown.Icon symbol="edit" fill />
                    <Span>Edit</Span>
                  </Dropdown.Item>
                  <Dropdown.Item onSelect={() => setCancelling(true)}>
                    <Dropdown.Icon symbol="close" fill />
                    <Span>Cancel</Span>
                  </Dropdown.Item>
                </Dropdown.Content>
              </Dropdown.Root>
            </Table.Cell>
          </MotionTRow>

          <Dialog.Root open={cancelling} onOpenChange={setCancelling}>
            <Dialog.Content title="Cancel shipment" description="Please confirm this action.">
              <Form
                className="flex flex-col gap-10"
                onSubmit={(e) => {
                  e.preventDefault();
                  setCancelling(false);
                  onCancel?.();
                }}
              >
                <Container className="flex flex-col gap-2">
                  <Span className="font-semibold">{shipment.id.slice(-8).toUpperCase()}</Span>
                  <Span className="text-sm inline-flex gap-2 items-center text-foreground-secondary">
                    <Span>
                      {shipment.from.city}, {shipment.from.state}
                    </Span>
                    <Icon symbol="arrow_right_alt" size={20} />
                    <Span>
                      {shipment.to.city}, {shipment.to.state}
                    </Span>
                  </Span>
                </Container>
                <Paragraph className="text-sm text-foreground-secondary">This shipment will be cancelled immediately and cannot be undone.</Paragraph>
                <Container className="flex w-full items-center justify-end gap-4">
                  <Dialog.Close asChild>
                    <Button type="button" intent="ghost">
                      Keep
                    </Button>
                  </Dialog.Close>
                  <Button type="submit" intent="destructive">
                    Cancel
                  </Button>
                </Container>
              </Form>
            </Dialog.Content>
          </Dialog.Root>
        </>
      );
    }
  ),

  Card: createElement<typeof Fragment, { shipment: ShipmentType; onUpdate?: () => void; onCancel?: () => void }>(
    ({ shipment, onUpdate, onCancel }, _) => {
      const [cancelling, setCancelling] = useState(false);

      return (
        <>
          <Accordion.Root collapsible type="single" className="flex flex-col bg-input outline outline-line-faint rounded-4xl">
            <Accordion.Item value={shipment.id}>
              <Accordion.Header>
                <Accordion.Trigger className="group flex items-center w-full p-10">
                  <Container className="flex flex-col items-start grow xl:gap-2">
                    <Container className="flex w-full items-center gap-2 justify-between">
                      <Span className="text-2xl xl:text-4xl font-bold uppercase">{shipment.id.slice(-8)}</Span>
                      <Icon symbol="keyboard_arrow_down" className="font-light transition group-data-[state=open]:rotate-180 duration-300" />
                    </Container>
                    <Span className="text-foreground-quaternary text-xs uppercase">2 Parcels • 2 Units</Span>
                  </Container>
                </Accordion.Trigger>
              </Accordion.Header>
              <Accordion.Content>
                <Shipment.Address title="Origin" address={shipment.from} className="pt-0!" />
                <Shipment.Separator />
                <Shipment.Address title="Destination" address={shipment.to} />
                <Container className="flex flex-col items-center gap-2.5 p-4">
                  <Button shape="pill" size="fill" onClick={onUpdate}>
                    <Span>Update</Span>
                  </Button>
                  <Button shape="pill" size="fill" onClick={() => setCancelling(true)} destructive>
                    Cancel
                  </Button>
                </Container>
              </Accordion.Content>
            </Accordion.Item>
          </Accordion.Root>

          <Dialog.Root open={cancelling} onOpenChange={setCancelling}>
            <Dialog.Content title="Cancel shipment" description="Please confirm this action.">
              <Form
                className="flex flex-col gap-10"
                onSubmit={(e) => {
                  e.preventDefault();
                  setCancelling(false);
                  onCancel?.();
                }}
              >
                <Container className="flex flex-col gap-2">
                  <Span className="font-semibold">{shipment.id.slice(-8).toUpperCase()}</Span>
                  <Span className="text-sm inline-flex gap-2 items-center text-foreground-secondary">
                    <Span>
                      {shipment.from.city}, {shipment.from.state}
                    </Span>
                    <Icon symbol="arrow_right_alt" size={20} />
                    <Span>
                      {shipment.to.city}, {shipment.to.state}
                    </Span>
                  </Span>
                </Container>
                <Paragraph className="text-sm text-foreground-secondary">This shipment will be cancelled immediately and cannot be undone.</Paragraph>
                <Container className="flex w-full items-center justify-end gap-4">
                  <Dialog.Close asChild>
                    <Button type="button" intent="ghost">
                      Keep
                    </Button>
                  </Dialog.Close>
                  <Button type="submit" intent="destructive">
                    Cancel
                  </Button>
                </Container>
              </Form>
            </Dialog.Content>
          </Dialog.Root>
        </>
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
          <Container className="grid grid-cols-1 sm:grid-cols-2 gap-10 py-10 px-[5px]">{children}</Container>
        </Accordion.Content>
      </Accordion.Item>
    );
  }),

  Creator: createElement<typeof Sheet.Root, { onCreate: (shipment: ShipmentType) => void }>(({ onCreate, ...props }, ref) => {
    const empty = (): Address => ({
      line1: "",
      city: "",
      state: "",
      zip: "",
      country: "USA"
    });

    const [from, setFrom] = useState<Address>(empty());
    const [to, setTo] = useState<Address>(empty());

    useEffect(() => {
      if (props.open) {
        setFrom(empty());
        setTo(empty());
      }
    }, [props.open]);

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

      toast.promise(Spatial.shipments.create({ from, to }), {
        loading: "Creating shipment…",
        success: async (response) => {
          if (response.error) {
            return {
              type: "error",
              message: "Create failed",
              description: "Something went wrong."
            };
          }

          await onCreate(response.data!);

          return { message: "Shipment created", description: response.data!.id };
        },
        error: { message: "Create failed", description: "Something went wrong." }
      });
    };

    return (
      <Sheet.Root {...props} ref={ref}>
        <Sheet.Content title="New Shipment" closeButton>
          <Form className="flex flex-col gap-10 sm:min-w-xl" onSubmit={submit}>
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

              <Shipment.Section value="payload" label="Payload" />
              <Shipment.Section value="status" label="Status" />
            </Accordion.Root>

            <Container className="flex gap-4">
              <Button type="submit">Create</Button>
              <Sheet.Close asChild>
                <Button intent="ghost">Cancel</Button>
              </Sheet.Close>
            </Container>
          </Form>
        </Sheet.Content>
      </Sheet.Root>
    );
  }),

  Editor: createElement<typeof Sheet.Root, { shipment: ShipmentType; onEdit: (shipment: ShipmentType) => void }>(
    ({ shipment, onEdit, ...props }, ref) => {
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

        toast.promise(Spatial.shipments.update({ ...shipment, from, to }), {
          loading: "Updating shipment…",
          success: async (response) => {
            if (response.error) {
              return {
                type: "error",
                message: "Update failed",
                description: "Something went wrong."
              };
            }

            await onEdit(response.data!);

            return { message: "Shipment updated", description: response.data!.id };
          },
          error: { message: "Update failed", description: "Something went wrong." }
        });
      };

      return (
        <Sheet.Root {...props} ref={ref}>
          <Sheet.Content title={shipment.id.slice(-8).toUpperCase()} closeButton>
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

                <Shipment.Section value="payload" label="Payload" />
                <Shipment.Section value="status" label="Status" />
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
    }
  )
};
