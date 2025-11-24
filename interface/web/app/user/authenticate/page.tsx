// Copyright © Spatial Corporation. All rights reserved.

import { Field, Form, Main } from "@sptlco/matter";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  return (
    <Main className="flex items-center justify-center h-screen">
      <Form className="flex flex-col w-full max-w-md space-y-12">
        <Field type="text" label="Email Address" placeholder="you@sptlco.com" description="This is your account's email address." />
        <Field type="password" label="Passphrase" placeholder="This is my passphrase" description="This is your account's email address." />
        <Field type="text" label="Sigma" placeholder="Enter a sigma value" description="This is your account's email address." />
      </Form>
    </Main>
  );
}
