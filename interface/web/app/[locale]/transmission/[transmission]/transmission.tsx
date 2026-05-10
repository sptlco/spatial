// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Button, Container, createElement, Field, Form, Icon, Logo, Span, Spinner, toast } from "@sptlco/design";
import { clsx } from "clsx";
import { FormEvent, useEffect, useRef, useState } from "react";

export const Transmission = createElement<typeof Container, { transmission: string }>(({ transmission, ...props }, ref) => {
  const [passphrase, setPassphrase] = useState("");
  const [loading, setLoading] = useState(false);
  const [blob, setBlob] = useState<string | null>(null);
  const video = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    return () => {
      if (blob) {
        URL.revokeObjectURL(blob);
      }
    };
  }, [blob]);

  const stream = async (e: FormEvent) => {
    e.preventDefault();

    setLoading(true);

    toast.promise(Spatial.transmissions.stream(transmission, passphrase), {
      loading: "Securing transmission",
      success: (blob) => {
        setLoading(false);
        setBlob(URL.createObjectURL(blob));

        return { message: "Transmission secured" };
      },
      error: (error) => {
        setLoading(false);

        return {
          message: "Something went wrong",
          description: error.message
        };
      }
    });
  };

  const render = () => {
    if (blob) {
      return <video ref={video} src={blob} controls autoPlay className="w-full max-w-3xl rounded-lg" />;
    }

    return (
      <>
        <Logo mode="symbol" className="w-16" />
        <Form onSubmit={stream} className="group relative w-full max-w-sm gap-4 items-end flex flex-col">
          <Field
            type="password"
            placeholder="Enter a passphrase"
            value={passphrase}
            onChange={(e) => setPassphrase(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Escape") {
                e.preventDefault();
                setPassphrase("");
              }
            }}
            className="pr-12"
            autoFocus
          />
          <Button type="submit" className={clsx("shrink-0! size-7! p-0! absolute right-1.5 bottom-1.5", "group-focus-within:bg-button-highlight!")}>
            {loading ? <Spinner className="size-3" /> : <Icon symbol="arrow_right_alt" size={20} />}
          </Button>
        </Form>
      </>
    );
  };

  return (
    <Container {...props} ref={ref} className="flex flex-col gap-10 w-screen h-screen items-center justify-center">
      {render()}
    </Container>
  );
});
