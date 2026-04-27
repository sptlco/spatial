// Copyright © Spatial Corporation. All rights reserved.

/**
 * A page that collects essential information such as a
 * customer's name, email address, and payment details.
 * @param props Configurable options for the page.
 * @returns A checkout page.
 */
export default async function Page(props: { params: Promise<{ locale: string; order: string }> }) {
  const order = (await props.params).order;

  // ...

  return order;
}
