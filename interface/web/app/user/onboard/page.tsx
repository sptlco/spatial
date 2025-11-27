// Copyright © Spatial Corporation. All rights reserved.

import { Button, Field, Form, Icon, Logo, Main, Span } from "@sptlco/matter";

/**
 * A page that onboards the current user.
 * @returns A user onboarding page.
 */
export default function Onboarding() {
  return (
    <Main className="flex flex-col px-10 items-center justify-center h-screen space-y-20">
      <Logo className="fill-current h-8" mode="symbol" />
      <Form className="flex flex-col w-full max-w-md space-y-8">
        <Field type="email" label="User ID" placeholder="you@sptlco.com" />
        <Field type="text" label="Full name" placeholder="John" />
        <Button className="w-full mt-8" intent="secondary" shape="pill">
          <Span>Create account</Span>
          <Icon symbol="arrow_right_alt" />
        </Button>
      </Form>
    </Main>
  );
}
