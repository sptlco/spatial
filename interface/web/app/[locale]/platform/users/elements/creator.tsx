// Copyright © Spatial Corporation. All rights reserved.

import { Button, Container, createElement, Field, Form, Sheet } from "@sptlco/design";

export const Creator = createElement<typeof Sheet.Content>((props, ref) => (
  <Sheet.Content title="Create a new user" description="Accounts enable friends, family, or colleagues to access the platform." closeButton>
    <Form className="flex flex-col gap-10 w-full max-w-sm">
      <Field
        type="text"
        id="name"
        name="name"
        placeholder="name@company.com"
        label="Email address"
        description="The user will sign into the platform with this email address."
        inset={false}
      />
      <Container className="flex items-center gap-2">
        <Button>Create</Button>
        <Sheet.Close asChild>
          <Button intent="secondary">Cancel</Button>
        </Sheet.Close>
      </Container>
    </Form>
  </Sheet.Content>
));
