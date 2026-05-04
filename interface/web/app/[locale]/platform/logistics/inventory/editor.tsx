// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Address, Asset, AssetType, AssetView } from "@sptlco/data";
import { FormEvent, useEffect, useState } from "react";
import { KeyedMutator } from "swr";

import { Button, Container, createElement, Field, Form, Label, Sheet, Span, Spinner, toast } from "@sptlco/design";

const emptyLocation = (): Address => ({ line1: "", line2: "", city: "", state: "", zip: "", country: "" });

export const Editor = createElement<typeof Sheet.Content, { asset: AssetView | null; mutate: KeyedMutator<AssetView[]> }>(
  ({ asset, mutate, ...props }, ref) => {
    const [type, setType] = useState<AssetType>("physical");
    const [quantity, setQuantity] = useState(1);
    const [location, setLocation] = useState<Address>(emptyLocation());
    const [variants, setVariants] = useState<Record<string, string>>();
    const [metadata, setMetadata] = useState<Record<string, string>>();
    const [saving, setSaving] = useState(false);

    const physical = type === "physical";

    const setLoc = (key: keyof Address, value: string) => {
      setLocation((prev) => ({ ...prev, [key]: value }));
    };

    useEffect(() => {
      if (!asset) return;
      setType(asset.asset.type);
      setQuantity(asset.asset.quantity);
      setLocation(asset.asset.location ?? emptyLocation());
      setVariants(asset.asset.variants ?? undefined);
      setMetadata(asset.asset.metadata ?? undefined);
    }, [asset]);

    const save = async (e: FormEvent) => {
      e.preventDefault();

      if (!asset) return;

      setSaving(true);

      const updated: Asset = {
        ...asset.asset,
        type,
        quantity,
        location: physical ? location : undefined,
        variants: variants ?? {},
        metadata: metadata ?? {}
      };

      toast.promise(Spatial.assets.update(updated), {
        loading: "Saving asset",
        description: "We are saving your changes to this asset.",
        success: async (saved) => {
          setSaving(false);
          await mutate((prev = []) => prev.map((item) => (item.asset.id === saved.id ? { ...item, asset: saved } : item)), false);

          return {
            message: "Asset saved",
            description: (
              <>
                Updated <Span className="font-semibold">{asset.model.name}</Span>.
              </>
            )
          };
        },
        error: (error) => {
          setSaving(false);
          return { message: "Something went wrong", description: error.message };
        }
      });
    };

    return (
      <Sheet.Content {...props} ref={ref} title={asset?.model.name ?? "Edit asset"} description="Update the details of this asset." closeButton>
        <Form className="flex flex-col w-full sm:w-screen sm:max-w-sm gap-10" onSubmit={save}>
          <Field
            type="text"
            id="model"
            name="model"
            label="Model"
            description="The Stripe product this asset is associated with. To change the model, you must recreate the asset."
            value={asset?.asset.model ?? ""}
            readOnly
            inset={false}
          />
          <Field type="text" id="lot" name="lot" label="Lot" value={asset?.asset.id ?? ""} readOnly inset={false} />
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
            disabled={saving}
            inset={false}
          />
          <Field
            type="number"
            id="quantity"
            name="quantity"
            label="Quantity"
            value={quantity}
            onChange={(e) => setQuantity(Number(e.target.value))}
            disabled={saving}
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
                disabled={saving}
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
                disabled={saving}
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
                disabled={saving}
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
                disabled={saving}
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
                disabled={saving}
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
                disabled={saving}
                inset={false}
              />
            </>
          )}
          <Field
            type="meta"
            id="variants"
            name="variants"
            label="Variants"
            metadata={variants}
            onValueChange={setVariants}
            disabled={saving}
            inset={false}
            required={false}
          />
          <Field
            type="meta"
            id="metadata"
            name="metadata"
            label="Metadata"
            metadata={metadata}
            onValueChange={setMetadata}
            disabled={saving}
            inset={false}
            required={false}
          />
          <Container className="flex items-center gap-4">
            <Button type="submit" disabled={saving}>
              Save
            </Button>
            <Sheet.Close asChild>
              <Button intent="ghost">Cancel</Button>
            </Sheet.Close>
            {saving && <Spinner className="size-5 text-foreground-secondary" />}
          </Container>
        </Form>
      </Sheet.Content>
    );
  }
);
