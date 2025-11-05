// Copyright © Spatial Corporation. All rights reserved.

import { readFile } from "fs/promises";
import sizeOf from "image-size";
import { ISizeCalculationResult } from "image-size/types/interface";
import NextImage, { ImageProps as NextImageProps } from "next/image";
import { join } from "path";
import { FC, ReactNode } from "react";

type ImageProps = NextImageProps & {
  src: string;
  width?: number;
  height?: number;
};

/**
 * Create a new image component.
 * @param props Configurable options for the component.
 * @returns An image component.
 */
export const Image: FC<ImageProps> = async ({ src, width, height, ...props }: ImageProps): Promise<ReactNode> => {
  if (!width || !height) {
    let buffer: Buffer;
    let dimensions: ISizeCalculationResult;

    if (src.startsWith("http")) {
      buffer = Buffer.from(await (await fetch(src)).arrayBuffer());
    } else if (!process.env.CI && process.env.VERCEL_ENV && process.env.NODE_ENV === "production") {
      buffer = Buffer.from(await (await fetch(`https://${process.env.VERCEL_URL}${src}`)).arrayBuffer());
    } else {
      buffer = await readFile(join(process.cwd(), "public", src));
    }

    dimensions = sizeOf(buffer);

    if (!dimensions.width || !dimensions.height) {
      throw new Error("Failed to compute the size of the image.");
    }

    width = dimensions.width;
    height = dimensions.height;
  }

  let base = process.env.NEXT_PUBLIC_BASE_URL;
  let alt: string | null = props.alt ?? null;
  let factor = 1;

  if (props.alt) {
    const match = props.alt.match(/(.*?)(?: \[(\d+)%\])?$/);

    if (match) {
      alt = match[1].trim();
      factor = match[2] ? parseInt(match[2], 10) / 100 : 1;
    }
  }

  return (
    <NextImage src={base ? `${base.replace(/\/$/, "")}/${src.replace(/^\//, "")}` : src} width={width * factor} height={height * factor} {...props} />
  );
};
