// Copyright © Spatial Corporation. All rights reserved.

import { Assignment } from "@sptlco/data";
import { Controller } from "..";

/**
 * A controller for assignments.
 */
export class AssignmentController extends Controller {
  /**
   * Get a list of assignments.
   * @returns A list of assignments.
   */
  public list = () => {
    return this.get<Assignment[]>("assignments/list");
  };
}
