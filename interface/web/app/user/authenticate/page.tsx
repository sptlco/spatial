// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Spatial } from "@sptlco/client";
import { Button, Container, Dialog, Field, Form, Hidden, Icon, Logo, Main, OTP, Span, Spinner } from "@sptlco/design";
import cookies from "js-cookie";
import { FormEvent, useState } from "react";

type AuthenticationStep = "idle" | "requesting" | "confirming" | "verifying" | "authenticated";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  const [step, setStep] = useState<AuthenticationStep>("idle");

  const [userId, setUserId] = useState("");
  const [code, setCode] = useState("");

  const loading = step === "requesting" || step === "verifying";
  const open = step === "confirming" || step === "verifying";

  const request = async (e: FormEvent) => {
    e.preventDefault();

    if (step !== "idle") {
      return;
    }

    setStep("requesting");

    const response = await Spatial.keys.create({ user: userId });

    if (response.error) {
      // ...

      setStep("idle");
      return;
    }

    setStep("confirming");
  };

  const verify = async () => {
    if (step !== "confirming") {
      return;
    }

    setStep("verifying");

    const response = await Spatial.sessions.create({ user: userId, key: code });

    if (response.error) {
      // ...

      setCode("");
      setStep("confirming");
      return;
    }

    cookies.set("spatial.session", response.data.token, {
      path: "/",
      secure: true,
      expires: 365
    });

    setStep("authenticated");
  };

  return (
    <Main className="flex flex-col px-10 items-center justify-center h-screen space-y-20">
      <Logo className="fill-current h-8" mode="symbol" />
      <Form className="flex flex-col w-full max-w-sm space-y-8" onSubmit={request}>
        <Field
          type="text"
          label="User ID"
          name="userId"
          id="userId"
          placeholder="email@address.com"
          disabled={step !== "idle"}
          value={userId}
          onChange={(e) => setUserId(e.target.value)}
        />
        <Button type="submit" className="w-full mt-8" intent="ghost" shape="pill">
          <Span>Continue</Span>
          {loading ? <Spinner className="size-4 m-1 text-foreground-tertiary" /> : <Icon symbol="arrow_right_alt" />}
        </Button>
      </Form>
      <Dialog.Root
        open={open}
        onOpenChange={(open) => {
          if (!open) {
            setStep("idle");
            setCode("");
          }
        }}
      >
        <Dialog.Portal>
          <Dialog.Overlay>
            <Dialog.Close />
          </Dialog.Overlay>
          <Dialog.Content className="flex flex-col px-10 items-center space-y-20">
            <Hidden>
              <Dialog.Title>Enter your code</Dialog.Title>
              <Dialog.Description>We just emailed you a 4-digit code.</Dialog.Description>
            </Hidden>
            <Logo className="fill-current h-8" mode="symbol" />
            <Field
              type="otp"
              maxLength={4}
              label="Authorization code"
              value={code}
              onValueChange={(value) => setCode(value.toUpperCase())}
              onComplete={verify}
              disabled={step === "verifying"}
              autoFocus
              description={
                <>
                  You received an authorization code.
                  <br />
                  To continue, enter it above.
                </>
              }
              containerClassName="items-center text-center"
              className="justify-center"
            >
              <OTP.Group className="justify-center">
                <OTP.Slot index={0} />
                <OTP.Slot index={1} />
                <OTP.Slot index={2} />
                <OTP.Slot index={3} />
              </OTP.Group>
            </Field>
            <Container className="inline-flex w-full h-6 items-center justify-center">
              {step === "verifying" && <Spinner className="size-6 text-foreground-tertiary" />}
            </Container>
          </Dialog.Content>
        </Dialog.Portal>
      </Dialog.Root>
    </Main>
  );
}
