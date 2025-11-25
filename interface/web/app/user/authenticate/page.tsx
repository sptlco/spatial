// Copyright © Spatial Corporation. All rights reserved.

import { Button, Field, Form, Icon, Logo, Main, Span } from "@sptlco/matter";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  return (
    <Main className="flex flex-col px-10 items-center justify-center h-screen space-y-20">
      <Logo className="fill-current h-8" mode="symbol" />
      <Form className="flex flex-col w-full max-w-md space-y-8">
        <Field type="text" label="User ID" placeholder="you@sptlco.com" />
        <Field type="password" label="Passphrase" placeholder="Your passphrase" />
        <Button className="w-full mt-8" intent="secondary" shape="pill">
          <Span>Sign in</Span>
          <Icon symbol="arrow_right_alt" />
        </Button>
      </Form>
    </Main>
  );
}
