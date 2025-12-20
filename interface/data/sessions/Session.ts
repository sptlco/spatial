// Copyright © Spatial Corporation. All rights reserved.

import { Record } from "..";

export type Session = Record<{
  user: string;
  agent: string;
  token: string;
  expires: number;
}>;
