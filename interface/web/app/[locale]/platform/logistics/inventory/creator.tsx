// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { AssetType, AssetView, CreateAssetOptions, Version } from "@sptlco/data";
import { FormEvent, useState } from "react";
import { KeyedMutator } from "swr";

import { Button, Container, createElement, Field, Form, Label, Sheet, Span, Spinner, toast } from "@sptlco/design";

export const Creator = createElement<typeof Sheet.Content, { mutate: KeyedMutator<AssetView[]> }>(({ mutate, ...props }, ref) => {
  const [type, setType] = useState<AssetType>("physical");
  const [model, setModel] = useState("");
  const [product, setProduct] = useState("");
  const [lot, setLot] = useState("");
  const [quantity, setQuantity] = useState(1);
  const [location, setLocation] = useState({ line1: "", line2: "", city: "", state: "", zip: "", country: "" });
  const [metadata, setMetadata] = useState<Record<string, string>>();
  const [creating, setCreating] = useState(false);

  const setLoc = (key: keyof typeof location, value: string) => {
    setLocation((prev) => ({ ...prev, [key]: value }));
  };

  const physical = type === "physical";

  const create = async (e: FormEvent) => {
    e.preventDefault();
    setCreating(true);

    const options: CreateAssetOptions = {
      type,
      model,
      product,
      lot: lot || undefined,
      quantity,
      location: physical ? location : undefined,
      metadata
    };

    toast.promise(Spatial.assets.create(options), {
      loading: "Creating asset",
      description: "We are creating a new asset with the information you provided.",
      success: async (asset) => {
        setCreating(false);
        await mutate((prev = []) => [...prev, asset], false);

        return {
          message: "Asset created",
          description: (
            <>
              Created <Span className="font-semibold">{asset.product.name}</Span>.
            </>
          )
        };
      },
      error: (error) => {
        setCreating(false);
        return { message: "Something went wrong", description: error.message };
      }
    });
  };

  return (
    <Sheet.Content {...props} ref={ref} title="New asset" description="Register a physical or digital asset." closeButton>
      <Form className="flex flex-col w-full sm:w-screen sm:max-w-sm gap-10" onSubmit={create}>
        <Field
          type="text"
          id="product"
          name="product"
          label="Product ID"
          description="This is the Stripe product this asset is associated with. Controls the name, pricing, and metadata shown to customers."
          placeholder="prod_QkT7mXvR3nJw9L"
          value={product}
          onChange={(e) => setProduct(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="option"
          multiple={false}
          id="type"
          name="type"
          label="Type"
          options={[
            { value: "physical", label: "Physical" },
            { value: "digital", label: "Digital" }
          ]}
          selection={type}
          onValueChange={(value) => setType(value as AssetType)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="text"
          id="model"
          name="model"
          label="Model"
          placeholder="SDK-001"
          value={model}
          onChange={(e) => setModel(e.target.value)}
          disabled={creating}
          inset={false}
        />
        <Field
          type="text"
          id="lot"
          name="lot"
          label="Lot"
          placeholder="LOT-2025-A"
          value={lot}
          onChange={(e) => setLot(e.target.value)}
          disabled={creating}
          inset={false}
          required={false}
        />
        <Field
          type="number"
          id="quantity"
          name="quantity"
          label="Quantity"
          value={quantity}
          onChange={(e) => setQuantity(Number(e.target.value))}
          disabled={creating}
          inset={false}
          min={0}
          step={0.01}
        />
        {physical && (
          <>
            <Label className="text-hint uppercase font-extrabold text-xs">Location</Label>
            <Field
              type="text"
              id="line1"
              name="line1"
              label="Address line 1"
              placeholder="Street address"
              value={location.line1}
              onChange={(e) => setLoc("line1", e.target.value)}
              disabled={creating}
              inset={false}
            />
            <Field
              type="text"
              id="line2"
              name="line2"
              label="Address line 2"
              placeholder="Suite, floor, etc."
              value={location.line2}
              onChange={(e) => setLoc("line2", e.target.value)}
              disabled={creating}
              inset={false}
              required={false}
            />
            <Field
              type="text"
              id="city"
              name="city"
              label="City"
              placeholder="Seattle"
              value={location.city}
              onChange={(e) => setLoc("city", e.target.value)}
              disabled={creating}
              inset={false}
            />
            <Field
              type="text"
              id="state"
              name="state"
              label="State"
              placeholder="WA"
              value={location.state}
              onChange={(e) => setLoc("state", e.target.value)}
              disabled={creating}
              inset={false}
            />
            <Field
              type="text"
              id="zip"
              name="zip"
              label="ZIP"
              placeholder="98101"
              value={location.zip}
              onChange={(e) => setLoc("zip", e.target.value)}
              disabled={creating}
              inset={false}
            />
            <Field
              type="text"
              id="country"
              name="country"
              label="Country"
              placeholder="US"
              value={location.country}
              onChange={(e) => setLoc("country", e.target.value)}
              disabled={creating}
              inset={false}
            />
          </>
        )}
        <Field
          type="meta"
          id="metadata"
          name="metadata"
          label="Metadata"
          metadata={metadata}
          onValueChange={setMetadata}
          disabled={creating}
          inset={false}
          required={false}
        />
        <Container className="flex items-center gap-4">
          <Button type="submit" disabled={creating || !model || !product}>
            Create
          </Button>
          <Sheet.Close asChild>
            <Button intent="ghost">Cancel</Button>
          </Sheet.Close>
          {creating && <Spinner className="size-5 text-foreground-secondary" />}
        </Container>
      </Form>
    </Sheet.Content>
  );
});
