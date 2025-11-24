// Copyright © Spatial Corporation. All rights reserved.

import { Button, Field, Form, Icon, Main, Span } from "@sptlco/matter";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  return (
    <Main className="flex items-center justify-center h-screen bg-background-surface">
      <Form className="flex flex-col w-full max-w-md space-y-12">
        <Field type="text" label="Email Address" placeholder="you@sptlco.com" description="This is your account's email address." />
        <Field type="password" label="Passphrase" placeholder="This is my passphrase" description="This is your account's email address." />
        <Field type="text" label="Sigma" placeholder="Enter a sigma value" description="This is your account's email address." />
        <Button className="w-full" intent="secondary" shape="pill">
          <Span>Continue</Span>
          <Icon symbol="arrow_right_alt" />
        </Button>
      </Form>
    </Main>
  );
}
