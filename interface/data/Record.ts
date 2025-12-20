// Copyright © Spatial Corporation. All rights reserved.

export type Record<S> = {
  id: string;
  created: number;
} & S;
