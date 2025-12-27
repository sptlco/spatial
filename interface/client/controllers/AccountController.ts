// Copyright © Spatial Corporation. All rights reserved.

import { Account } from "@sptlco/data";
import { Controller } from "..";

export class AccountController extends Controller {
  /**
   * Identify the current user.
   * @returns The user's account.
   */
  public me = async () => {
    return this.get<Account>("accounts/me");
  };
}
