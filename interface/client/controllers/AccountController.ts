// Copyright © Spatial Corporation. All rights reserved.

import { Account, CreateAccountOptions } from "@sptlco/data";
import { Controller } from "..";

export class AccountController extends Controller {
  /**
   * Identify the current user.
   * @returns The user's account.
   */
  public me = async () => {
    return this.get<Account>("accounts/me");
  };

  /**
   * Create a new account.
   * @param options Configurable options for the account.
   * @returns An account.
   */
  public create = async (options: CreateAccountOptions) => {
    return this.post<Account>("accounts", options);
  };

  /**
   * Update the current account.
   * @param update An account update.
   * @returns The updated account.
   */
  public update = async (update: Account) => {
    return this.patch<Account>("accounts/me", update);
  };

  public del = async (id: string) => {
    return this.delete(`accounts/${id}`);
  };
}
