// Copyright © Spatial Corporation. All rights reserved.

import { Transmission } from "./transmission";

/**
 * A page displaying a transmission.
 */
export default async function Page(props: { params: Promise<{ locale: string; transmission: string }> }) {
  return <Transmission transmission={(await props.params).transmission} />;
}
