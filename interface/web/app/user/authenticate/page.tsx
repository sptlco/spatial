// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Button, Container, Field, Form, Icon, Logo, Main, OTP, Span, Spinner } from "@sptlco/design";
import { Spatial } from "@sptlco/server";
import { FormEvent, useRef, useState } from "react";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  const form = useRef<HTMLFormElement>(null);

  const [step, setStep] = useState(1);
  const [processing, setProcessing] = useState(false);

  const [userId, setUserId] = useState("");
  const [code, setCode] = useState("");

  const advance = async (e: FormEvent) => {
    e.preventDefault();

    switch (step) {
      case 1:
        // Generate a one-time password (OTP), and send it to the
        // email that the user provided.

        if (processing) {
          return;
        }

        setProcessing(true);

        const response = await Spatial.sessions.create({ user: userId });

        setProcessing(false);

        if (response.error) {
          // ...
        }

        setStep(2);

        break;
      case 2:
        // Check the code against the server, and authenticate the user
        // if the code matches.

        if (processing) {
          return;
        }

        setProcessing(true);

        // ...

        break;
    }
  };

  const render = () => {
    switch (step) {
      case 1:
        return (
          <>
            <Field
              type="text"
              label="User ID"
              name="userId"
              id="userId"
              placeholder="you@sptlco.com"
              disabled={processing}
              value={userId}
              onChange={(e) => setUserId(e.target.value)}
            />
            <Button type="submit" className="w-full mt-8" intent="ghost" shape="pill">
              <Span>Continue</Span>
              {processing ? <Spinner className="size-4 m-1 text-foreground-tertiary" /> : <Icon symbol="arrow_right_alt" />}
            </Button>
          </>
        );
      case 2:
        return (
          <>
            <Field
              type="otp"
              maxLength={4}
              label="Authorization code"
              value={code}
              onValueChange={setCode}
              onComplete={() => form.current?.dispatchEvent(new Event("submit", { cancelable: true, bubbles: true }))}
              disabled={processing}
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
              {processing && <Spinner className="size-6 text-foreground-tertiary" />}
            </Container>
          </>
        );
    }
  };

  return (
    <Main className="flex flex-col px-10 items-center justify-center h-screen space-y-20">
      <Logo className="fill-current h-8" mode="symbol" />
      <Form ref={form} className="flex flex-col w-full max-w-sm space-y-8" onSubmit={advance}>
        {render()}
      </Form>
    </Main>
  );
}
