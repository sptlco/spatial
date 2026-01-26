// Copyright © Spatial Corporation. All rights reserved.

import { Resource } from "..";

export type Session = Resource<{
  user: string;
  agent: string;
  token: string;
  expires: number;
}>;
