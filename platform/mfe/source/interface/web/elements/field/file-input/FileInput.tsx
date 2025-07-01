// Copyright © Spatial Corporation. All rights reserved.

"use client";

import clsx from "clsx";

import {
  Button,
  Div,
  Element,
  Field,
  FileInputProps,
  Node,
  Span,
} from "@spatial/elements";
import { useEffect, useRef, useState } from "react";

/**
 * Create a new file input element.
 * @param props Configurable options for the element.
 * @returns A file input element.
 */
export const FileInput: Element<FileInputProps> = (
  props: FileInputProps,
): Node => {
  const [files, setFiles] = useState<FileList | null>(null);

  return (
    <Field
      name={props.name}
      label={props.label}
      description={props.description}
      disabled={props.disabled}
      className={props.className}
    >
      <Div
        className={clsx(
          "relative",
          "cursor-pointer",
          "w-fit",
          "space-x-1/2u flex items-center",
        )}
      >
        <Button size="small" intent="secondary" roundness="pill">
          Select a file
        </Button>
        <Span
          children={files?.item(0)?.name || "No file selected"}
          className={clsx(
            "text-s",
            "whitespace-nowrap",
            "overflow-hidden text-ellipsis",
          )}
        />
        <input
          type="file"
          name={props.name}
          accept={props.accept}
          onChange={(e) => {
            setFiles(e.target.files);
            props.onChange?.(e.target.files || new FileList());
          }}
          className={clsx(
            "size-full",
            "!m-0",
            "absolute left-0 top-0",
            "cursor-pointer opacity-0",
          )}
        />
      </Div>
    </Field>
  );
};
