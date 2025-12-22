// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Button, Container, Dialog, Field, Form, H1, Hidden, Icon, Logo, Main, OTP, Paragraph, resolve, Span, Spinner } from "@sptlco/design";
import cookies from "js-cookie";
import { useSearchParams } from "next/navigation";
import { FormEvent, useEffect, useState } from "react";

const KEY_LENGTH = 4;

type AuthenticationState = "idle" | "requesting" | "confirming" | "verifying" | "authenticated";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  const params = useSearchParams();

  const [state, setState] = useState<AuthenticationState>("idle");

  const [userId, setUserId] = useState("");
  const [code, setCode] = useState("");

  const loading = state === "requesting" || state === "verifying";
  const open = state === "confirming" || state === "verifying";

  const request = async (e: FormEvent) => {
    e.preventDefault();

    if (state !== "idle") {
      return;
    }

    setState("requesting");

    const response = await Spatial.keys.create({ user: userId });

    if (response.error) {
      // ...

      setState("idle");
      return;
    }

    setState("confirming");
  };

  const verify = async () => {
    if (state !== "confirming") {
      return;
    }

    setState("verifying");

    const response = await Spatial.sessions.create({ user: userId, key: code });

    if (response.error) {
      // ...

      setCode("");
      setState("confirming");
      return;
    }

    cookies.set("spatial.session", response.data.token, {
      path: "/",
      secure: true,
      expires: 365
    });

    setState("authenticated");
  };

  const next = () => {
    let href = resolve("/");
    let param = params.get("next");

    if (param) {
      href = atob(param);
    }

    window.location.href = href;
  };

  useEffect(() => {
    switch (state) {
      case "authenticated":
        next();
        break;
    }
  }, [state]);

  return (
    <Main className="flex px-10 items-center justify-center h-screen">
      <Form className="flex flex-col items-center w-full max-w-sm space-y-10" onSubmit={request}>
        <Container className="flex flex-col w-full items-center space-y-10">
          <Logo className="fill-current h-8" mode="symbol" />
          <Container className="w-full flex flex-col text-center">
            <H1 className="font-medium">Secure Access</H1>
            <Paragraph className="text-foreground-secondary">Sign in to continue to Spatial.</Paragraph>
          </Container>
        </Container>
        <Field
          type="text"
          label="Email Address"
          name="userId"
          id="userId"
          placeholder="name@company.com"
          description={<>Learn about our approach to secure, passwordless authentication in the documentation.</>}
          disabled={state !== "idle"}
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
        />
        <Button type="submit" className="w-full" intent="secondary">
          <Span>Continue</Span>
          {loading ? <Spinner className="size-4 m-1 text-foreground-tertiary" /> : <Icon symbol="arrow_right_alt" />}
        </Button>
      </Form>
      <Dialog.Root
        open={open}
        onOpenChange={(open) => {
          if (!open) {
            setState("idle");
            setCode("");
          }
        }}
      >
        <Dialog.Portal>
          <Dialog.Overlay>
            <Dialog.Close />
          </Dialog.Overlay>
          <Dialog.Content className="flex flex-col px-10 items-center space-y-10">
            <Hidden>
              <Dialog.Title>Verify your identity</Dialog.Title>
              <Dialog.Description>We&apos;ve sent a one-time verification code to your email address.</Dialog.Description>
            </Hidden>
            <Logo className="fill-current h-8" mode="symbol" />
            <Field
              type="otp"
              maxLength={KEY_LENGTH}
              label="Verification Code"
              value={code}
              onValueChange={(value) => setCode(value.toUpperCase())}
              onComplete={verify}
              disabled={state === "verifying"}
              autoFocus
              description={<>A one-time verification code has been sent to your email address. You may request a new code if necessary.</>}
              containerClassName="items-center text-center"
              className="justify-center"
            >
              <OTP.Group className="justify-center">
                {[...Array(KEY_LENGTH)].map((_, index) => (
                  <OTP.Slot key={index} index={index} />
                ))}
              </OTP.Group>
            </Field>
            <Container className="inline-flex w-full h-6 items-center justify-center">
              {state === "verifying" && <Spinner className="size-6 text-foreground-tertiary" />}
            </Container>
          </Dialog.Content>
        </Dialog.Portal>
      </Dialog.Root>
    </Main>
  );
}
