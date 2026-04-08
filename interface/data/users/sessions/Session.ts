// Copyright © Spatial Corporation. All rights reserved.

import { Schema } from "../..";

export type Session = Schema<{
  user: string;
  agent: string;
  token: string;
  expires: number;
}>;
