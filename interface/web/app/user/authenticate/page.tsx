// Copyright © Spatial Corporation. All rights reserved.

import { Button, Form, Main } from "@sptlco/matter";

/**
 * A page that authenticates the current user.
 * @returns A user authentication page.
 */
export default function Authentication() {
  return (
    <Main className="flex items-center justify-center h-screen">
      <Form className="flex flex-col">
        <Button intent="primary">Sign in</Button>
      </Form>
    </Main>
  );
}
