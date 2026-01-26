// Copyright © Spatial Corporation. All rights reserved.

export type Resource<S> = {
  id: string;
  created: number;
  metadata: Record<string, string>;
} & S;
