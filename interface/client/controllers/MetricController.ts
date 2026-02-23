// Copyright © Spatial Corporation. All rights reserved.

import { Metric, WriteMetricOptions } from "@sptlco/data";
import { Controller } from "..";

export class MetricController extends Controller {
  /**
   * Read data points for a given metric.
   * @param name The metric's name.
   * @param from Optional start date (UTC).
   * @param to Optional end date (UTC).
   * @param limit Optional maximum number of records.
   * @returns Matching data points.
   */
  public read = (name: string, from?: Date, to?: Date, limit?: number, resolution?: string) => {
    const params = new URLSearchParams();

    if (from) {
      params.append("from", from.toISOString());
    }

    if (to) {
      params.append("to", to.toISOString());
    }

    if (limit != null) {
      params.append("limit", limit.toString());
    }

    if (resolution) {
      params.append("resolution", resolution);
    }

    const query = params.toString();

    return this.get<Metric[]>(query ? `metrics/${name}?${query}` : `metrics/${name}`);
  };

  /**
   * Create a new metric data point.
   * @param options Configurable options for the metric.
   * @returns The created metric data point.
   */
  public write = (options: WriteMetricOptions) => {
    return this.post<Metric>("metrics", options);
  };
}
