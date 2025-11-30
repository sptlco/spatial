// Copyright © Spatial Corporation. All rights reserved.

"use client";

import { Button, Field, Form, Icon, Logo, Main, OTP, Span, Spinner } from "@sptlco/design";
import { useState } from "react";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  const [step, setStep] = useState(2);

  const Step = () => {
    switch (step) {
      case 1:
        const [loading, setLoading] = useState(false);

        return (
          <>
            <Field type="text" label="User ID" placeholder="you@sptlco.com" disabled={loading} />
            <Button className="w-full mt-8" intent="ghost" shape="pill" onClick={() => setStep(2)}>
              <Span>Sign in</Span>
              {loading ? <Spinner className="size-4 m-1 text-foreground-tertiary" /> : <Icon symbol="arrow_right_alt" />}
            </Button>
          </>
        );
      case 2:
        return (
          <>
            <Field type="otp" maxLength={4}>
              <OTP.Group>
                <OTP.Slot index={0} />
                <OTP.Slot index={1} />
                <OTP.Slot index={2} />
                <OTP.Slot index={3} />
              </OTP.Group>
            </Field>
          </>
        );
    }
  };

  return (
    <Main className="flex flex-col px-10 items-center justify-center h-screen space-y-20">
      <Logo className="fill-current h-8" mode="symbol" />
      <Form className="flex flex-col w-full max-w-md space-y-8">
        <Step />
      </Form>
    </Main>
  );
}
